using System;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Urd.RemoteConfig;

namespace Urd.SaveLoad
{
    [Serializable]
    public class RemoteConfigurationServiceProviderUnityRemoteConfig : IRemoteConfigurationServiceProvider
    {

        public void Init()
        {
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        }
        
        private void ApplyRemoteConfig(ConfigResponse configResponse)
        {
            // TODO check the data
            Debug.Log(RemoteConfigService.Instance.appConfig);
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