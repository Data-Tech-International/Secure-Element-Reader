using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SecureElementReader.DependencyInjection;
using SecureElementReader.Interfaces;
using SecureElementReader.ViewModels;
using SecureElementReader.ViewModels.Interfaces;
using SecureElementReader.Views;
using Splat;
using System;

namespace SecureElementReader
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            LoadLanguage();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DataContext = GetRequiredService<IMainWindowViewModel>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = DataContext,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();

        private static void LoadLanguage()
        {
            var menuView = GetRequiredService<IMenuViewModel>();
            menuView.StartUpTranslate();
        }
    }
}
