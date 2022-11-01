using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SecureElementReader.App.Interfaces;

namespace SecureElementReader.App.Services
{
    public class MainWindowProvider : IMainWindowProvider
    {
        public Window GetMainWindow()
        {
            var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime;

            return lifetime?.MainWindow;
        }
    }
}
