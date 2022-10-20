using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SecureElementReader.App.Models;
using SecureElementReader.App.ViewModels.Interfaces;
using SecureElementReader.App.ViewModels.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels
{
    public class CertDetailsViewModel : ViewModelBase, ICertDetailsViewModel
    {
        private readonly IDialogService dialogService;

        
        public ICommand VerificationInfoCommand { get; }

        [Reactive]
        public CertDetailsModel? CertDetailsModel { get; set; }

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

        [Reactive]
        public string SubmitInternalDataStatus { get; set; }

        [Reactive]
        public string PendingCommandsStatus { get; set; }

        public CertDetailsViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            VerificationInfoCommand = ReactiveCommand.CreateFromTask(ShowVerificationInfoDialogAsync);            
        }

        private Task ShowVerificationInfoDialogAsync()
        {
            return dialogService.ShowDialogAsync(nameof(VerificationInfoDialogViewModel));
        }

        public void ClearForm()
        {
            CertDetailsModel = null;
            SeCertValid = false;
            SeCertInvalid = false;
            PkiCertValid = false;
            PkiCertInvalid = false;
            BtnVisibility = false;
            SubmitInternalDataStatus = String.Empty;
            PendingCommandsStatus = String.Empty;
        }

        public void SetVerifyFields()
        {
            if (CertDetailsModel.SEVerify)
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

            if (CertDetailsModel.PkiVerifyed)
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

        public void SetStatusFileds(string internalDataStatus, string commandsStatus)
        {
            SubmitInternalDataStatus = internalDataStatus;
            PendingCommandsStatus = commandsStatus;
        }
    }
}
