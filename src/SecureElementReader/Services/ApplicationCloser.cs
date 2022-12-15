using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using SecureElementReader.Interfaces;

namespace SecureElementReader.Services
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
