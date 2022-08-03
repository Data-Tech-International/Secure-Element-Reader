
using ReactiveUI;
using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Input;
using MessageBox.Avalonia.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System.Windows.Input;
using System;
using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using Microsoft.Extensions.PlatformAbstractions;

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
