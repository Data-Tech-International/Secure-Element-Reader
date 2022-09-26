
using ReactiveUI;
using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using System.Diagnostics;
using Microsoft.Extensions.PlatformAbstractions;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels
{
    public class AboutDialogViewModel : DialogViewModelBase
    {

        public string AssemblyVersion => GetAssemblyVersion();

        public string GetAssemblyVersion()
        {
            return PlatformServices.Default.Application.ApplicationVersion;
        }

        public ICommand GoToGitHubRepository { get; }

        public AboutDialogViewModel()
        {
            GoToGitHubRepository = ReactiveCommand.Create(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Process.Start(new ProcessStartInfo("https://github.com/Data-Tech-International/Secure-Element-Reader") { UseShellExecute = true });
        }
    }
}
