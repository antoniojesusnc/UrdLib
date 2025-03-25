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
            //Debug.Log(json);
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
                return JsonConvert.DeserializeObject<T>(loadedValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public bool TryLoad<T>(string key, out T loadedValue)
        {
            loadedValue = default;
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }
            
            try
            {
                loadedValue = JsonConvert.DeserializeObject<T>(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LoadAndPopulate<T>(string key, ref T valeToPopulate)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }
            
            var jsonDefaultValue = Newtonsoft.Json.JsonConvert.SerializeObject(valeToPopulate);
            var loadedValue = PlayerPrefs.GetString(key, jsonDefaultValue);
            if (loadedValue == jsonDefaultValue)
            {
                return false;
            }

            try
            {
                Newtonsoft.Json.JsonConvert.PopulateObject(loadedValue, valeToPopulate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

    }
}