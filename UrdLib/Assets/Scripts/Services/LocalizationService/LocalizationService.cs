using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using Urd.Services.Localization;

namespace Urd.Services
{
    [Serializable]
    public class LocalizationService : BaseService, ILocalizationService
    {     
        public override int LoadPriority => 50;

        private const string MAIN_TABLE_REFERENCE = "MainTable";
            
        public CultureInfo Language { get; private set; }
        public List<Locale> AvailableLanguages => LocalizationSettings.AvailableLocales.Locales;

        private IEventBusService _eventBusService;
        
        public override void Init()
        {
            base.Init();

            //_eventBusService = ServiceLocatorService.Get<IEventBusService>();
            //LoadLanguage();
        }
        private void LoadLanguage()
        {
            var initializationOperation = LocalizationSettings.InitializationOperation;
            initializationOperation.Completed += OnInitializeLocalization;
        }

        private void OnInitializeLocalization(AsyncOperationHandle<LocalizationSettings> task)
        {
            Language = task.Result.GetSelectedLocale().Identifier.CultureInfo;
            _eventBusService.Send(new EventOnLocalizationChanged());
        }

        public void ChangeLanguage(string languageCode)
        {
            var locale =
                AvailableLanguages.Find(
                    language => language.Identifier.Code.Equals(languageCode,
                                                                StringComparison.InvariantCultureIgnoreCase));
            if (locale != null)
            {
                LocalizationSettings.Instance.SetSelectedLocale(locale);
                _eventBusService.Send(new EventOnLocalizationChanged());
            }
        }

        public string Locate(string key)
        {
            TryLocate(key, out var value);
            return value;
        }

        public bool TryLocate(string key, out string value)
        {
            var stringReference = new LocalizedString()
            {
                TableReference = MAIN_TABLE_REFERENCE,
                TableEntryReference = key,
            };
            value = stringReference.GetLocalizedString();
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}