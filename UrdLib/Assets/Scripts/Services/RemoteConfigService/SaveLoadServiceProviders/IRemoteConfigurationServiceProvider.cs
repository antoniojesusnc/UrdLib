namespace Urd.RemoteConfig
{
    public interface IRemoteConfigurationServiceProvider
    {
        T Load<T>(string key, T defaultValue);
        bool HasKey(string key);
    }
}
