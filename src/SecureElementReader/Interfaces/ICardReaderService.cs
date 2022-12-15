using SecureElementReader.Models;
using System.Collections.Generic;

namespace SecureElementReader.Interfaces
{
    public interface ICardReaderService
    {
        IEnumerable<string> LoadReaders();
        CertDetailsModel GetCertDetails();
        VerifyPinModel VerifyPin(string pin);
        byte[] GetInternalData();
        byte[] GetAmountStatus();
        List<CommandsStatusResult> ProcessingCommand(List<Command> commands);
        void Disconnect();
    }
}
