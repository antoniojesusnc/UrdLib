using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.SaveLoad;

namespace Urd.Services
{
    [Serializable]
    public class SaveLoadService : BaseService, ISaveLoadService
    {
        [SerializeReference, SubclassSelector] private List<ISaveLoadServiceProvider> _providers = new();

        public override int LoadPriority => 30;

        public SaveLoadService()
        {
            _providers.Add(new SaveLoadServiceProviderPlayerPref());
        }
        
        public void Save<T>(Enum key, T value) => Save(key.ToString(), value);

        public void Save<T>(string key, T value)
        {
            for (int i = 0; i < _providers.Count; i++)
            {
                _providers[i].Save(key, value);
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