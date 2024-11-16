#if UNITY_ANDROID || UNITY_IOS

using System;
using Unity.Notifications;

namespace Urd.Notifications
{
    public interface INotificationModel
    {
        DateTime DeliveryDateTime { get; }
        bool CanShowNotification();
        Notification GetNotification();
    }
}
#endif