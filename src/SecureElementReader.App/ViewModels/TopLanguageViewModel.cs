using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.ViewModels
{
    public class TopLanguageViewModel : ViewModelBase, ITopLanguageViewModel
    {
        private readonly ILanguageManager _languageManager;
        private readonly ILocalizationService _localizationService;
        private readonly IMainWindowProvider _mainWindowProvider;

        private ObservableCollection<LanguageModel> _languages;

        [Reactive]
        public LanguageModel CurrentLanguage { get; set; }

        public IEnumerable<LanguageModel> Languages => _languages;

        public TopLanguageViewModel(ILanguageManager languageManager, 
            ILocalizationService localizationService, 
            IMainWindowProvider mainWindowProvider)
        {
            _languageManager = languageManager;
            _localizationService = localizationService;
            _mainWindowProvider = mainWindowProvider;   
            

            _languages = new ObservableCollection<LanguageModel>(GetSortedLanguages());

            var savedLanguage = _localizationService.GetSavedLanguage();
            var currentLanguage = _languageManager.CurrentLanguage;

            var languageCode = savedLanguage is null ? currentLanguage.Code : savedLanguage.Code;
            CurrentLanguage = GetLanguageOrDefault(languageCode);
        }

        public void SaveChanges()
        {
            _languageManager.SetLanguage(CurrentLanguage);            
            _localizationService.SaveLanguage(CurrentLanguage);
        }

        private IEnumerable<LanguageModel> GetSortedLanguages() =>
            _languageManager.AllLanguages.OrderBy(l => l.Name);

        private LanguageModel GetLanguageOrDefault(string languageCode)
            => Languages.SingleOrDefault(l => l.Code == languageCode) ?? _languageManager.DefaultLanguage;

        public Window GetMainWindow()
        {
            return _mainWindowProvider.GetMainWindow();
           
        }
    }
}
