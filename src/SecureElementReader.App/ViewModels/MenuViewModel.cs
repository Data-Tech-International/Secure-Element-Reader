using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.MarkupExtensions;
using ReactiveUI;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.ViewModels.Interfaces;
using SecureElementReader.App.ViewModels.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels
{
    public class MenuViewModel : ViewModelBase, IMenuViewModel
    {
        private readonly IDialogService dialogService;

        public ICommand ExitCommand { get; }

        public ICommand AboutCommand { get; }

        public MenuViewModel(
            IApplicationCloser applicationCloser,
            IDialogService dialogService)
        {
            this.dialogService = dialogService;

            ExitCommand = ReactiveCommand.Create(applicationCloser.CloseApp);
            AboutCommand = ReactiveCommand.CreateFromTask(ShowAboutDialogAsync);
        }

        private Task ShowAboutDialogAsync()
        {
            return dialogService.ShowDialogAsync(nameof(AboutDialogViewModel));
        }

        public void Translate(string targetLanguage)
        {
            var translations = App.Current.Resources.MergedDictionaries.OfType<ResourceInclude>().FirstOrDefault(x => x.Source?.OriginalString?.Contains("Translations.EN.axaml") ?? false);

            if (translations != null)
                App.Current.Resources.MergedDictionaries.Remove(translations);


            App.Current.Resources.MergedDictionaries.Add(
                new ResourceInclude()
                {
                    Source = new Uri($"avares://SecureElementReader.App/Properties/Translations.{targetLanguage}.axaml")
                });

        }
    }

}
