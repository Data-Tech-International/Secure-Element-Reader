using SecureElementReader.App.Models;

namespace SecureElementReader.App.Interfaces
{
    public interface ILocalizationService
    {
        LanguageModel GetSavedLanguage();

        void SaveLanguage(LanguageModel language);

        void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null);
    }
}
