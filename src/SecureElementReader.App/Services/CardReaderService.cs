using BerTlv;
using TaxCore.Libraries.Certificates;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Microsoft.Extensions.Logging;
using PCSC;
using PCSC.Iso7816;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace SecureElementReader.App.Services
{
    public class CardReaderService : ICardReaderService
    {
        private readonly IApduCommandService apduCommandService;
        private readonly ILogger logger;
        private IsoReader reader;
        private readonly IContextFactory contextFactory;
        private readonly IMainWindowProvider mainWindowProvider;

        public CardReaderService(IApduCommandService apduCommandService, ILogger logger, IContextFactory contextFactory, IMainWindowProvider mainWindowProvider)
        {
            this.apduCommandService = apduCommandService;
            this.logger = logger;
            this.contextFactory = contextFactory;
            this.mainWindowProvider = mainWindowProvider;
        }

        public VerifyPinModel VerifyPin(string pin)
        {
            var verifyPinModel = new VerifyPinModel();

            CheckPkiPin(pin, reader, verifyPinModel);
            CheckSePin(pin, reader, verifyPinModel);

            return verifyPinModel;
        }

        private VerifyPinModel CheckPkiPin(string pin, IIsoReader reader, VerifyPinModel returnModel)
        {
            var response = reader.Transmit(apduCommandService.SelectPKIApp());
            if (response.SW1 == 0x90)
            {
                response = reader.Transmit(apduCommandService.VerifyPkiPin(StringToByteArray(ToHax(pin))));
                if (response.SW1 == 0x90)
                {
                    returnModel.PkiPinSuccess = true;
                }
                else if (response.SW1 == 0x69 && response.SW2 == 0x83)
                {
                    returnModel.PKIAppletLocked = true;
                }
                else if (response.SW1 == 0x63)
                {
                    returnModel.PkiTrysLeft = response.SW2;
                }
                else
                {
                    returnModel.PkiPinSuccess = false;
                }
            }
            else
            {
                App.Current.TryFindResource("SelectPkiError", out var SelectPkiError);
                returnModel.ErrorList.Add($"{SelectPkiError}: {response.SW1} {response.SW2}");
            }

            return returnModel;
        }

        private VerifyPinModel CheckSePin(string pin, IIsoReader reader, VerifyPinModel returnModel)
        {
            var response = reader.Transmit(apduCommandService.SelectSEApp());
            if (response.SW1 == 0x90)
            {
                response = reader.Transmit(apduCommandService.VerifySEPin(ConvertPinToByteArray(pin)));
                if (response.SW1 == 0x90)
                {
                    returnModel.SePinSuccess = true;
                }
                else if (response.SW1 == 0x63 && response.SW2 == 0x10)
                {
                    returnModel.SEAppletLocked = true;
                }
                else
                {
                    returnModel.SePinSuccess = false;
                }
            }
            else
            {
                App.Current.TryFindResource("SelectSeError", out var SelectSeError);
                returnModel.ErrorList.Add($"{SelectSeError}: {response.SW1} {response.SW2}");
            }

            return returnModel;
        }

        public CertDetailsModel GetCertDetails()
        {
            var certDetailsModel = new CertDetailsModel();

            GetPkiDetails(reader, certDetailsModel);
            GetSeDetails(reader, certDetailsModel);

            return certDetailsModel;
        }

        private CertDetailsModel GetSeDetails(IIsoReader reader, CertDetailsModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reader.ReaderName))
                {
                    App.Current.TryFindResource("PleaseInseretCard", out var PleaseInseretCard);
                    model.ErrorCodes.Add($"{PleaseInseretCard}");
                    return model;
                }

                var response = reader.Transmit(apduCommandService.SelectSEApp());
                if (response.SW1 == 0x90)
                {
                    response = reader.Transmit(apduCommandService.GetSECert());
                    if (response.SW1 == 0x90)
                    {
                        var crt = response.GetData();
                        var c = new Certificate(crt);

                        if (!model.ReadPkiSuccess)
                        {
                            PopulateModel(c, model);
                        }
                        model.SEVerify = c.Verify();
                        VerifyChain(c, model, false);
                        model.SEReadSuccess = true;
                    }
                    else
                    {
                        App.Current.TryFindResource("GetSeCertError", out var GetSeCertError);
                        model.ErrorCodes.Add($"{GetSeCertError}: {response.SW1} {response.SW2}");
                    }
                }
                else
                {
                    App.Current.TryFindResource("SelectSeError", out var SelectSeError);
                    model.ErrorCodes.Add($"{SelectSeError}: {response.SW1} {response.SW2}");
                }
            }
            catch (Exception ex)
            {
                App.Current.TryFindResource("SeCertDetailsError", out var SeCertDetailsError);
                model.ErrorCodes.Add($"{SeCertDetailsError}: {ex.Message}");
                logger.LogError($"Failed to read SE data with error: {ex}");
            }

            return model;
        }

        private CertDetailsModel GetPkiDetails(IIsoReader reader, CertDetailsModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reader.ReaderName))
                {
                    return model;
                }

                var response = reader.Transmit(apduCommandService.SelectPKIApp());
                if (response.StatusWord == 0x9000)
                {
                    response = reader.Transmit(apduCommandService.GetPKICert());
                    if (response.SW1 == 0x90)
                    {
                        System.Threading.Thread.Sleep(200);
                        var crt = response.GetData();
                        ICollection<Tlv> tlvs = Tlv.ParseTlv(crt);
                        foreach (var item in tlvs)
                        {
                            var copyOfRange = item.Value.Skip(4);
                            try
                            {
                                var decc = DecompressZLIB(copyOfRange.ToArray());
                                var cert = new Certificate(decc);

                                PopulateModel(cert, model);
                                model.ReadPkiSuccess = true;
                                model.PkiVerifyed = cert.Verify();
                                VerifyChain(cert, model, true);

                                break;
                            }
                            catch (Exception ignored)
                            {
                            }
                        }
                    }
                    else
                    {
                        App.Current.TryFindResource("GetPkiCertError", out var GetPkiCertError);
                        model.ErrorCodes.Add($"{GetPkiCertError}: {response.SW1} {response.SW2}");
                    }
                }
                else
                {
                    App.Current.TryFindResource("SelectPkiError", out var SelectPkiError);
                    model.ErrorCodes.Add($"{SelectPkiError}: {response.SW1} {response.SW2}");
                }
            }
            catch (Exception ex)
            {
                App.Current.TryFindResource("PkiCertDetailsError", out var PkiCertDetailsError);
                model.ErrorCodes.Add($"{PkiCertDetailsError}: {ex.Message}");
                logger.LogError($"Failed to read PKI data with error: {ex}");
            }

            return model;
        }

        private void PopulateModel(Certificate c, CertDetailsModel model)
        {
            model.Subject = c.Subject;
            model.CommonName = c.CommonName;
            model.Organization = c.Organization;
            model.ExpiryDate = c.ExpiryDate;
            model.OrganizationUnit = c.OrganizationUnit;
            model.StreetAddress = c.StreetAddress;
            model.RequestedBy = c.RequestedBy;
            model.CertificateType = c.CertificateType;
            model.GivenName = c.GivenName;
            model.SurName = c.SurName;
            model.State = c.State;
            model.ExpiryDate = c.ExpiryDate;
            model.ApiUrl = c.ExtractTaxCoreApiUrl();
            model.Tin = c.ExtractTIN();
            model.UniqueIdentifier = c.UniqueIdentifier;
            model.IssuerName = c.IssuerName;
        }

        private byte[] DecompressZLIB(byte[] data)
        {
            byte[] finalBytesToSend;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    Inflater inflater = new Inflater();
                    inflater.SetInput(data);

                    byte[] buffer = new byte[1024];
                    while (!inflater.IsFinished)
                    {
                        int count = inflater.Inflate(buffer);
                        writer.Write(buffer, 0, count);
                    }
                }
                finalBytesToSend = stream.ToArray();
            }

            return finalBytesToSend;
        }

        public IEnumerable<string> LoadReaders()
        {
            string[]? szReaders = null;
            try
            {
                var context = contextFactory.Establish(SCardScope.User);

                szReaders = context.GetReaders();
                reader = new IsoReader(context);
                
                foreach (var sZReader in szReaders)
                {                   
                    reader.Connect(sZReader, SCardShareMode.Shared, SCardProtocol.T1);
                }

                System.Threading.Thread.Sleep(500);

                return szReaders;
            }
            catch (Exception)
            {
                return szReaders ?? Array.Empty<string>();
            }
        }

        public void Disconnect()
        {
            reader.Disconnect(SCardReaderDisposition.Leave);            
        }

        public byte[]? GetInternalData()
        {
            var response = reader.Transmit(apduCommandService.SelectSEApp());
            if (response.SW1 == 0x90)
            {
                response = reader.Transmit(apduCommandService.GetExportInternalData());
                if (response.SW1 == 0x90)
                {
                    return response.GetData();
                }
                else 
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public byte[]? GetAmountStatus()
        {
            var response = reader.Transmit(apduCommandService.SelectSEApp());
            if (response.SW1 == 0x90)
            {
                response = reader.Transmit(apduCommandService.AmountStatus());
                if (response.SW1 == 0x90)
                {
                    return response.GetData();
                }
                else if (response.SW1 == 0x63 && response.SW2 == 0x10) ;
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<CommandsStatusResult> ProcessingCommand(List<Command> commands)
        {
            var result = new List<CommandsStatusResult>();

            try
            {
                var response = reader.Transmit(apduCommandService.SelectSEApp());

                if (response.SW1 == 0x90 && response.SW2 == 0x00)
                {
                    foreach (var item in commands)
                    {
                        if (item.Type == Enums.CommandsType.ForwardSecureElementDirective)
                        {
                            response = reader.Transmit(apduCommandService.SECommand(Convert.FromBase64String(item.Payload)));
                        }
                        else
                        {
                            response = reader.Transmit(apduCommandService.FinishAudit(Convert.FromBase64String(item.Payload)));
                        }
                        
                        if (response.SW1 == 0x90)
                        {
                            result.Add(new CommandsStatusResult { CommandId = item.CommandId, DateAndTime = DateTime.UtcNow, Success = true });
                        }
                        else
                        {
                            result.Add(new CommandsStatusResult { CommandId = item.CommandId, DateAndTime = DateTime.UtcNow, Success = false });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        private byte[] ConvertPinToByteArray(string pin)
        {
            return pin.Select(c => byte.Parse(c.ToString())).ToArray();
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static string ToHax(string value)
        {
            return string.Join("",
                value.Select(c => String.Format("{0:X2}", Convert.ToInt32(c))));
        }

        private void VerifyChain(Certificate cert, CertDetailsModel model, bool IsPKI)
        {
            using (X509Chain chain = new X509Chain())
            {
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                chain.ChainPolicy.VerificationTime = DateTime.Now;

                chain.Build(cert);

                for (int i = 0; i < chain.ChainElements.Count; i++)
                {
                    X509ChainElement element = chain.ChainElements[i];

                    if (element.ChainElementStatus.Length != 0)
                    {
                        if (IsPKI)
                        {
                            model.PKIVerificationInfo += Environment.NewLine;
                            model.PKIVerificationInfo += "------------------------------";
                            model.PKIVerificationInfo += Environment.NewLine;
                            App.Current.TryFindResource("ErrorAtDepth", out var ErrorAtDepth);
                            model.PKIVerificationInfo += $"{ErrorAtDepth} {i}:{Environment.NewLine}{element.Certificate.Issuer}";
                            model.PKIVerificationInfo += Environment.NewLine;

                            foreach (var status in element.ChainElementStatus)
                            {
                                App.Current.TryFindResource("Status", out var Status);
                                model.PKIVerificationInfo += $"{Status}:{Environment.NewLine}{status.Status}";
                                model.PKIVerificationInfo += Environment.NewLine;
                                App.Current.TryFindResource("StatusInforamtion", out var StatusInforamtion);
                                model.PKIVerificationInfo += $"{StatusInforamtion}:{Environment.NewLine}{status.StatusInformation}";
                                model.PKIVerificationInfo += Environment.NewLine;
                            }

                            model.PKIVerificationInfo += "------------------------------";
                        }
                        else
                        {
                            model.SEVerificationInfo += Environment.NewLine;
                            model.SEVerificationInfo += "------------------------------";
                            App.Current.TryFindResource("ErrorAtDepth", out var ErrorAtDepth);
                            model.SEVerificationInfo += $"{ErrorAtDepth} {i}: {element.Certificate.Issuer}";
                            model.SEVerificationInfo += Environment.NewLine;

                            foreach (var status in element.ChainElementStatus)
                            {
                                model.SEVerificationInfo += $"{status.Status}: {status.StatusInformation}";
                                model.SEVerificationInfo += Environment.NewLine;
                            }
                            model.SEVerificationInfo += "------------------------------";
                        }
                    }
                }
            }
        }
    }
}
