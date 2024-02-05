using System;

namespace Urd.Services
{
    public interface ISaveLoadService : IBaseService
    {
        void Save<T>(Enum key, T value);
        void Save<T>(string key, T value);
        T Load<T>(string key, T defaultValue);
        T Load<T>(Enum key, T defaultValue);
        bool HasKey(string key);
    }
}