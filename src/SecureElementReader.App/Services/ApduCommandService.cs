using PCSC;
using PCSC.Iso7816;
using SecureElementReader.App.Enums;
using SecureElementReader.App.Interfaces;

namespace SecureElementReader.App.Services
{
    public class ApduCommandService : IApduCommandService
    {
        public CommandApdu SelectPKIApp()
        {
            return new CommandApdu(IsoCase.Case3Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectApp,
                Instruction = InstructionCode.SelectFile,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default,
                Data = new byte[] { 0xA0, 0x00, 0x00, 0x03, 0x97, 0x42, 0x54, 0x46, 0x59, 0x02, 0x01 }
            };
        }

        public CommandApdu SelectSEApp()
        {
            return new CommandApdu(IsoCase.Case4Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectApp,
                Instruction = InstructionCode.SelectFile,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default,
                Data = new byte[] { 0xA0, 0x00, 0x00, 0x07, 0x48, 0x46, 0x4A, 0x49, 0x2D, 0x54, 0x61, 0x78, 0x43, 0x6F, 0x72, 0x65 }
            };
        }

        public CommandApdu GetSECert()
        {
            return new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.ExportCert,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default
            };
        }

        public CommandApdu GetPKICert()
        {
            return new CommandApdu(IsoCase.Case3Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectApp,
                INS = (byte)ApduInstructions.ExportPKICert,
                P1 = (byte)ApduP1.PKIExport,
                P2 = (byte)ApduP2.PKIExport,
                Data = new byte[] { 0x5C, 0x00, 0x02, 0xDF, 0x02, 0x04, 0x00 }
            };
        }

        public CommandApdu VerifyPkiPin(byte[] pin)
        {
            return new CommandApdu(IsoCase.Case3Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectApp,
                INS = (byte)0x20,
                P1 = (byte)0x00,
                P2 = (byte)0x80,
                Data = pin
            };
        }

        public CommandApdu VerifySEPin(byte[] pin)
        {
            return new CommandApdu(IsoCase.Case3Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.PinVerify,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default,
                Data = pin
            };
        }

        public CommandApdu AmountStatus()
        {
            return new CommandApdu(IsoCase.Case2Short, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.AmountStatus,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default
            };
        }
        
        public CommandApdu GetExportInternalData()
        {
            return new CommandApdu(IsoCase.Case2Extended, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.ExportInternalData,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default
            };
        }

        public CommandApdu SECommand(byte[] seCommand)
        {
            return new CommandApdu(IsoCase.Case4Extended, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.SECommand,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default,
                Data = seCommand
            };
        }

        public CommandApdu FinishAudit(byte[] proofOfAudit)
        {
            return new CommandApdu(IsoCase.Case3Extended, SCardProtocol.T1)
            {
                CLA = (byte)ApduClasses.SelectCommand,
                INS = (byte)ApduInstructions.StopAudit,
                P1 = (byte)ApduP1.Default,
                P2 = (byte)ApduP2.Default,
                Data = proofOfAudit
            };
        }
    }
}
