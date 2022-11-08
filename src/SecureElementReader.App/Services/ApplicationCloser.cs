using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using SecureElementReader.App.Interfaces;

namespace SecureElementReader.App.Services
{
    public class ApplicationCloser : IApplicationCloser
    {
        public void CloseApp()
        {
            var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime;

            lifetime?.Shutdown();
        }
    }
}
