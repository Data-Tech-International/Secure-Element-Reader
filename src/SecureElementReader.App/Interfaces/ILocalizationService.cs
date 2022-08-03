using SecureElementReader.App.Models;

namespace SecureElementReader.App.Interfaces
{
    public interface ILocalizationService
    {
        LanguageModel GetSavedLanguage();

        void SaveLanguage(LanguageModel language);
    }
}
