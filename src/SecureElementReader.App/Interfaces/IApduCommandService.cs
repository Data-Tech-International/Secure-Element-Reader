using PCSC.Iso7816;

namespace SecureElementReader.App.Interfaces
{
    public interface IApduCommandService
    {
        CommandApdu SelectPKIApp();
        CommandApdu SelectSEApp();
        CommandApdu GetSECert();
        CommandApdu GetPKICert();
        CommandApdu VerifyPkiPin(byte[] pin);
        CommandApdu VerifySEPin(byte[] pin);
        CommandApdu AmountStatus();
        CommandApdu GetExportInternalData();
        CommandApdu SECommand(byte[] seCommand);    
    }
}
