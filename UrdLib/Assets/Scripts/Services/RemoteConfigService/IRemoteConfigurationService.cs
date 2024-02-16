using System;

namespace Urd.Services
{
    public interface IRemoteConfigurationService : IBaseService
    {
        T Load<T>(string key, T defaultValue);
        T Load<T>(Enum key, T defaultValue);
        bool HasKey(string key);
    }
}