using System;
using Unity.Notifications;
using UnityEngine;

namespace Urd.Notifications
{
    [Serializable]
    public class NotificationByTimeModel : INotificationModel
    {
        [SerializeField]
        private float _delayTimeInMinutes;

        [SerializeField]
        private string _notificationText;

        public DateTime DeliveryDateTime => DateTime.UtcNow.AddMinutes(_delayTimeInMinutes);

        public Notification GetNotification()
        {
            var notification = new Notification();
            notification.Text = _notificationText;

            return notification;
        }
    }
}