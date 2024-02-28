namespace Urd.RemoteConfig
{
    public interface IRemoteConfigurationServiceProvider
    {
        void Init();
        T Load<T>(string key, T defaultValue);
        bool HasKey(string key);
    }
}
