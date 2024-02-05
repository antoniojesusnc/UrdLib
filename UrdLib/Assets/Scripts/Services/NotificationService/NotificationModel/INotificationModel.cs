using System;
using Unity.Notifications;

public interface INotificationModel
{
    DateTime DeliveryDateTime { get; }
    Notification GetNotification();
}
