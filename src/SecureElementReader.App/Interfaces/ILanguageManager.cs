using SecureElementReader.App.Models;
using System.Collections.Generic;

namespace SecureElementReader.App.Interfaces
{
    public interface ILanguageManager
    {
        LanguageModel CurrentLanguage { get; }

        LanguageModel DefaultLanguage { get; }

        IEnumerable<LanguageModel> AllLanguages { get; }

        void SetLanguage(string languageCode);

        void SetLanguage(LanguageModel languageModel);
    }
}
