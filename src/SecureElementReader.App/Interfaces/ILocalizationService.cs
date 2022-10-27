using SecureElementReader.App.Models;

namespace SecureElementReader.App.Interfaces
{
    public interface ILocalizationService
    {
        void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null);
    }
}
