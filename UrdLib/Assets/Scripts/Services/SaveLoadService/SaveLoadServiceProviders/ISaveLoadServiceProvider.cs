using System;

namespace Urd.SaveLoad
{
    public interface ISaveLoadServiceProvider
    {
        void Save<T>(string key, T value);
        T Load<T>(string key, T defaultValue);
        bool HasKey(string key);
    }
}
