using SecureElementReader.App.Extensions;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SecureElementReader.App.Services
{
    public class LanguageManager : ILanguageManager
    {
        private readonly LanguagesConfiguration _configuration;
        private readonly Dictionary<string, LanguageModel> _availableLanguages;

        public LanguageModel DefaultLanguage { get; }

        public LanguageModel CurrentLanguage => CreateLanguageModel(Thread.CurrentThread.CurrentUICulture);

        public IEnumerable<LanguageModel> AllLanguages => _availableLanguages.Values;

        public LanguageManager(LanguagesConfiguration configuration)
        {
            DefaultLanguage = CreateLanguageModel(CultureInfo.GetCultureInfo("en"));
            _configuration = configuration;
            _availableLanguages = new Dictionary<string, LanguageModel>(GetAvailableLanguages());           
        }

        public void SetLanguage(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                throw new ArgumentException($"{nameof(languageCode)} can't be empty.");
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languageCode);
        }

        public void SetLanguage(LanguageModel languageModel) => SetLanguage(languageModel.Code);

        private Dictionary<string, LanguageModel> GetAvailableLanguages() =>
            _configuration
                .AvailableLocales
                .Select(locale => CreateLanguageModel(new CultureInfo(locale)))
                .ToDictionary(lm => lm.Code, lm => lm);

        private LanguageModel CreateLanguageModel(CultureInfo cultureInfo) =>
            cultureInfo is null
                ? DefaultLanguage
                : new LanguageModel(cultureInfo.EnglishName, cultureInfo.NativeName.ToTitleCase(),
                    cultureInfo.TwoLetterISOLanguageName);
    }
}
