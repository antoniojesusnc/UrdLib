using System;

namespace Urd.SaveLoad
{
    public interface ISaveLoadServiceProvider
    {
        void Save<T>(string key, T value);
        T Load<T>(string key, T defaultValue);
        bool TryLoad<T>(string key, out T loadedValue);
        bool HasKey(string key);
        bool LoadAndPopulate<T>(string key, ref T valeToPopulate);
    }
}
