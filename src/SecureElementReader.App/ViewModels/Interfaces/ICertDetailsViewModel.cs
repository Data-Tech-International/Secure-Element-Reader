using SecureElementReader.App.Models;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface ICertDetailsViewModel
    {
        CertDetailsModel? CertDetailsModel { get; set; }       
        ICommand VerificationInfoCommand { get; }
        void SetStatusFields(string internalStatus, string commandsStatus);
        void ClearForm();
        void SetVerifyFields();
    }
}
