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
        public string SeVerify { get; set; }

        [Reactive]
        public string PkiVerify { get; set; }

        [Reactive]
        public IBrush SeColor { get; set; }

        [Reactive]
        public IBrush PkiColor { get; set; }

        [Reactive]
        public bool BtnVisibility { get; set; }

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
            SeVerify = String.Empty;
            PkiVerify = String.Empty;
            BtnVisibility = false;
        }

        public void SetVerifyFields()
        {
            if (CertDetailsModel.SEVerify)
            {
                SeVerify = Properties.Resources.SeCertValid;
                SeColor = Brushes.Green;
                BtnVisibility = false;
            }
            else
            {
                SeVerify = Properties.Resources.SeCertInvalid;
                SeColor = Brushes.Red;
                BtnVisibility = true;
            }

            if (CertDetailsModel.PkiVerifyed)
            {
                PkiVerify = Properties.Resources.PkiCertValid;
                PkiColor = Brushes.Green;
                BtnVisibility = false;
            }
            else
            {
                PkiVerify = Properties.Resources.PkiCertInvalid;
                PkiColor = Brushes.Red;
                BtnVisibility = true;
            }
        }
    }
}
