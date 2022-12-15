namespace SecureElementReader.Interfaces
{
    public interface ILocalizationService
    {
        void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null);
    }
}
