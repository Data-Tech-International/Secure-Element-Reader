using SecureElementReader.App.Extensions;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.Models.Configurations;
using System;
using System.Globalization;
using System.IO;

namespace SecureElementReader.App.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly SelectedLanguageConfiguration _configuration;

        public LocalizationService(SelectedLanguageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LanguageModel GetSavedLanguage()
        {           
            return CreateFrom(new CultureInfo(_configuration.Language));
        }

        public void SaveLanguage(LanguageModel languageModel)
        {
            if (languageModel is null)
            {
                throw new ArgumentNullException(nameof(languageModel));
            }

            if (string.IsNullOrEmpty(languageModel.Name))
            {
                throw new ArgumentException($"{nameof(languageModel.Name)} can't be empty.");
            }

            if (string.IsNullOrEmpty(languageModel.Code))
            {
                throw new ArgumentException($"{nameof(languageModel.Code)} can't be empty.");
            }

            _configuration.Language = languageModel.Code;

            SetAppSettingValue(nameof(_configuration.Language), languageModel.Code);            
        }

        private static void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null)
        {
            if (appSettingsJsonFilePath == null)
            {
                appSettingsJsonFilePath = Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            }

            var json = File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            jsonObj["SelectedLanguage"][key] = value;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(appSettingsJsonFilePath, output);
        }

        private static LanguageModel CreateFrom(CultureInfo cultureInfo) =>
                cultureInfo is null
                    ? null
                    : new LanguageModel(cultureInfo.EnglishName, cultureInfo.NativeName.ToTitleCase(),
                    cultureInfo.TwoLetterISOLanguageName);
        
    }
}
