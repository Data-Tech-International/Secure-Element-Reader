using ReactiveUI;
using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using SecureElementReader.App.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels
{
    public class VerificationInfoDialogViewModel : DialogViewModelBase, IVerificationInfoDialog
    {
        public string VerificationDetails { get; set; }
        public string TapUrl { get; set; }

        public ICommand CloseButton { get; }

        public ICommand GoToTAP { get; set; }

        public ICertDetailsViewModel CertDetailsViewModel { get; }

        public VerificationInfoDialogViewModel(ICertDetailsViewModel certDetailsViewModel)
        {
            CertDetailsViewModel = certDetailsViewModel;
            PopulateDialog();            
        }

       
        private void PopulateDialog()
        {            
            VerificationDetails = CertDetailsViewModel?.CertDetailsModel?.PKIVerificationInfo;
            TapUrl = CertDetailsViewModel?.CertDetailsModel?.ApiUrl.Replace("api","tap");
            GoToTAP = ReactiveCommand.Create(OnButtonTAP);
        }

        public VerificationInfoDialogViewModel()
        {
            CloseButton = ReactiveCommand.Create(ButtonClose);
        }

        private void ButtonClose()
        {
            Close();
        }

        private void OnButtonTAP()
        {
            Process.Start(new ProcessStartInfo("https://tap.sandbox.suf.purs.gov.rs/Help/view/1048069196/Инсталирање-RCA-и-ICA-сертификата/sr-Cyrl-RS") { UseShellExecute = true });

        }
    }
}
