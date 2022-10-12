using Avalonia.Media;
using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface ICertDetailsViewModel
    {
        CertDetailsModel? CertDetailsModel { get; set; }       
        ICommand VerificationInfoCommand { get; }
        void SetStatusFileds(string internalStatus, string commandsStatus);
        void ClearForm();
        void SetVerifyFields();
    }
}
