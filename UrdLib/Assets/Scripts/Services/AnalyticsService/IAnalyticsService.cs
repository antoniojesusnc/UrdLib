namespace Urd.Services
{
    public interface IAnalyticsService : IBaseService
    {
        void LogEvent(string eventKey, string eventValue);
        bool TryGetProvider<T>(out T fireBaseProvider) where T : class, IAnalyticsServiceProvider;
    }
}