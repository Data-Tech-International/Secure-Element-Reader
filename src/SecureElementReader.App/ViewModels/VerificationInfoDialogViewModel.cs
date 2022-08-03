using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using SecureElementReader.App.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.ViewModels
{
    public class VerificationInfoDialogViewModel : DialogViewModelBase, IVerificationInfoDialog
    {
        public string VerificationDetails { get; set; }
        public string TapUrl { get; set; }

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
        }
    }
}
