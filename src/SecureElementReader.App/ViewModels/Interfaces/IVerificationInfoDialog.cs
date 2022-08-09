using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface IVerificationInfoDialog
    {
        string VerificationDetails { get; set; }
        string TapUrl { get; set; }

        public ICommand GoToTAP { get; set; }
    }
}
