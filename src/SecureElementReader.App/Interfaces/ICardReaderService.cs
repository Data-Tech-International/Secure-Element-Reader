using SecureElementReader.App.Models;
using System.Collections.Generic;

namespace SecureElementReader.App.Interfaces
{
    public interface ICardReaderService
    {
        IEnumerable<string> LoadReaders();
        CertDetailsModel GetCertDetails();
        VerifyPinModel VerifyPin(string pin);
    }
}
