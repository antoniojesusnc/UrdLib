using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Localization;

namespace Urd.Services
{
    public interface ILocalizationService : IBaseService
    {
        CultureInfo Language { get; }
        List<Locale> AvailableLanguages { get; }
        void ChangeLanguage(string languageCode);
        string Locate(string key);
        bool TryLocate(string key, out string value);
    }
}