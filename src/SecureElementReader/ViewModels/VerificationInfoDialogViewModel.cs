using ReactiveUI;
using SecureElementReader.ViewModels.Implementations.Dialogs;
using SecureElementReader.ViewModels.Interfaces;
using System.Diagnostics;
using System.Windows.Input;

namespace SecureElementReader.ViewModels
{
    public class VerificationInfoDialogViewModel : DialogViewModelBase, IVerificationInfoDialog
    {
        public string VerificationDetails { get; set; }
        public string TapUrl { get; set; }

        public ICommand CloseButton { get; }

        public ICommand GoToTap { get; set; }

        public ICertDetailsViewModel CertDetailsViewModel { get; }

        public VerificationInfoDialogViewModel(ICertDetailsViewModel certDetailsViewModel)
        {
            CertDetailsViewModel = certDetailsViewModel;
            PopulateDialog();
        }


        private void PopulateDialog()
        {
            VerificationDetails = CertDetailsViewModel?.CertDetailsModel?.PkiVerificationInfo;
            TapUrl = CertDetailsViewModel?.CertDetailsModel?.ApiUrl.Replace("api", "tap");
            GoToTap = ReactiveCommand.Create(OnButtonTap);
        }

        public VerificationInfoDialogViewModel()
        {
            CloseButton = ReactiveCommand.Create(ButtonClose);
        }

        private void ButtonClose()
        {
            Close();
        }

        private void OnButtonTap()
        {
            Process.Start(
                new ProcessStartInfo("https://tap.sandbox.suf.purs.gov.rs/Help/view/1048069196/Инсталирање-RCA-и-ICA-сертификата/sr-Cyrl-RS") 
                { 
                    UseShellExecute = true 
                });
        }
    }
}
