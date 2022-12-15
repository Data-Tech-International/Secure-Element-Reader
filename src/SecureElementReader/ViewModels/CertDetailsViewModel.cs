using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SecureElementReader.Models;
using SecureElementReader.ViewModels.Interfaces;
using SecureElementReader.ViewModels.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.ViewModels
{
    public class CertDetailsViewModel : ViewModelBase, ICertDetailsViewModel
    {
        private readonly IDialogService _dialogService;

        public delegate void StatusAction(string internalDataStatus, string commandsStatus);
        public event StatusAction SetStatus;

        public delegate void ClearAction();
        public event ClearAction ClearFields;

        public ICommand VerificationInfoCommand { get; }

        [Reactive]
        public CertDetailsModel CertDetailsModel { get; set; }

        [Reactive]
        public bool BtnVisibility { get; set; }

        [Reactive]
        public bool SeCertValid { get; set; }

        [Reactive]
        public bool SeCertInvalid { get; set; }

        [Reactive]
        public bool PkiCertValid { get; set; }

        [Reactive]
        public bool PkiCertInvalid { get; set; }

        public CertDetailsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            VerificationInfoCommand = ReactiveCommand.CreateFromTask(ShowVerificationInfoDialogAsync);
        }

        private Task ShowVerificationInfoDialogAsync()
        {
            return _dialogService.ShowDialogAsync(nameof(VerificationInfoDialogViewModel));
        }

        public void ClearForm()
        {
            CertDetailsModel = null;
            SeCertValid = false;
            SeCertInvalid = false;
            PkiCertValid = false;
            PkiCertInvalid = false;
            BtnVisibility = false;

            ClearFields?.Invoke();
        }

        public void SetVerifyFields()
        {
            if (CertDetailsModel.SeVerify)
            {
                BtnVisibility = false;
                SeCertValid = true;
                SeCertInvalid = false;
            }
            else
            {
                BtnVisibility = true;
                SeCertValid = false;
                SeCertInvalid = true;
            }

            if (CertDetailsModel.PkiVerified)
            {
                BtnVisibility = false;
                PkiCertValid = true;
                PkiCertInvalid = false;
            }
            else
            {
                BtnVisibility = true;
                PkiCertValid = false;
                PkiCertInvalid = true;
            }
        }

        public void SetStatusFields(string internalDataStatus, string commandsStatus)
        {
            SetStatus?.Invoke(internalDataStatus, commandsStatus);
        }
    }
}
