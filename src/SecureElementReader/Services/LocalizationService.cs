using SecureElementReader.Interfaces;
using SecureElementReader.Models.Configurations;
using System.IO;

namespace SecureElementReader.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly SelectedLanguageConfiguration _configuration;

        public LocalizationService(SelectedLanguageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null)
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


    }
}
