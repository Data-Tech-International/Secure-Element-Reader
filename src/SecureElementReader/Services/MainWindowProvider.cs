﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SecureElementReader.Interfaces;

namespace SecureElementReader.Services
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
