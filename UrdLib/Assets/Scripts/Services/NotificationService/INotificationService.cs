#if UNITY_ANDROID || UNITY_IOS

namespace Urd.Services
{
    public interface INotificationService : IBaseService
    {
        void CancelNotifications();
        void SetConfig(NotificationsConfig notificationsConfig);
    }
}
#endif