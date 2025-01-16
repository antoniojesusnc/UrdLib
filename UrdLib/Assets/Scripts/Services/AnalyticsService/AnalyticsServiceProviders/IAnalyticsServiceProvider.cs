namespace Urd.Services
{
    public interface IAnalyticsServiceProvider
    {
        void Init();
        void LogEvent(string eventKey, string eventValue);
    }
}