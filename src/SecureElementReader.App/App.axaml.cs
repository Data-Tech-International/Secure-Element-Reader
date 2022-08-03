using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SecureElementReader.App.DependencyInjection;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.ViewModels;
using SecureElementReader.App.ViewModels.Interfaces;
using SecureElementReader.App.Views;
using Splat;
using System;

namespace SecureElementReader.App
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
            var localizationService = GetRequiredService<ILocalizationService>();
            var savedLanguage = localizationService.GetSavedLanguage();
            if (savedLanguage != null)
            {
                var languageManager = GetRequiredService<ILanguageManager>();

                languageManager.SetLanguage(savedLanguage.Code);
            }
        }
    }
}
