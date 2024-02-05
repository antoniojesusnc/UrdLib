using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Urd.SaveLoad
{
    [Serializable]
    public class SaveLoadServiceProviderPlayerPref : ISaveLoadServiceProvider
    {
        public void Save<T>(string key, T value)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, json);
        }

        public T Load<T>(string key, T defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }
            
            var jsonDefaultValue = Newtonsoft.Json.JsonConvert.SerializeObject(defaultValue);
            var loadedValue = PlayerPrefs.GetString(key, jsonDefaultValue);
            if (loadedValue == jsonDefaultValue)
            {
                return defaultValue;
            }

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(loadedValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}