using Avalonia.Markup.Xaml.MarkupExtensions;
using ReactiveUI;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models.Configurations;
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
        private readonly SelectedLanguageConfiguration _configuration;
        private readonly IDialogService dialogService;
        private readonly ILocalizationService _localizationService;


        public ICommand ExitCommand { get; }

        public ICommand AboutCommand { get; }

        public MenuViewModel(
            IApplicationCloser applicationCloser,
            IDialogService dialogService,
            ILocalizationService localizationService,
            SelectedLanguageConfiguration configuration)
        {
            this.dialogService = dialogService;
            _localizationService = localizationService;
            _configuration = configuration;

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

            _localizationService.SetAppSettingValue(nameof(_configuration.Language), targetLanguage);// PRREKO _conf.Lang IZAZVATI PREVOD PRI STARTU
        }

        public void StartUpTranslate()
        {
            Translate(_configuration.Language);
        }

    }

}
