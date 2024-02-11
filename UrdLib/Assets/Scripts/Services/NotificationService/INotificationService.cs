namespace Urd.Services
{
    public interface INotificationService : IBaseService
    {
        void CancelNotifications();
        void SetConfig(NotificationServiceConfig notificationServiceConfig);
    }
}