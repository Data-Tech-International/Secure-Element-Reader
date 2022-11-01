using System.Windows.Input;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface IVerificationInfoDialog
    {
        string VerificationDetails { get; set; }
        string TapUrl { get; set; }

        public ICommand GoToTap { get; set; }
    }
}
