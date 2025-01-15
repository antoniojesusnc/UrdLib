namespace Urd.Services
{
    public interface IAnalyticsService : IBaseService
    {
        void LogEvent(string eventKey, string eventValue);
    }
}