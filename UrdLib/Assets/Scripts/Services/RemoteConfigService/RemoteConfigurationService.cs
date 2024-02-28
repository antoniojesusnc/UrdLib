using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.RemoteConfig;
using Urd.SaveLoad;

namespace Urd.Services
{
    [Serializable]
    public class RemoteConfigurationService : BaseService, IRemoteConfigurationService
    {
        [SerializeReference, SubclassSelector] private List<IRemoteConfigurationServiceProvider> _providers = new();

        public override int LoadPriority => 50;

        public RemoteConfigurationService()
        {
            _providers.Add(new RemoteConfigurationServiceProviderUnityRemoteConfig());
        }

        public override void Init()
        {
            base.Init();
            
            for (int i = 0; i < _providers.Count; i++)
            {
                _providers[i].Init();
            }
        }

        public T Load<T>(Enum key, T defaultValue) => Load(key.ToString(), defaultValue);

        public T Load<T>(string key, T defaultValue)
        {
            // TODO provider load all

            if (_providers.Count > 0)
            {
                return _providers[0].Load(key, defaultValue);
            }

            return default(T);
        }

        public bool HasKey(Enum key) => HasKey(key.ToString());
        public bool HasKey(string key)
    {
            for (int i = 0; i < _providers.Count; i++)
            {
                if (_providers[i].HasKey(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}