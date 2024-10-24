using System;
using Unity.Notifications;
using UnityEngine;
using UnityEngine.Localization;

namespace Urd.Notifications
{
    public abstract class NotificationModel : INotificationModel
    {
        [SerializeField]
        private LocalizedString _notificationTittle;
        
        [SerializeField]
        protected LocalizedString _notificationText;

        public DateTime DeliveryDateTime => GetDeliveryTime();
        protected abstract DateTime GetDeliveryTime();

        public abstract bool CanShowNotification();
        public Notification GetNotification()
        {
            var notification = new Notification();
            notification.Title = _notificationTittle.GetLocalizedString();
            notification.Text = _notificationText.GetLocalizedString();

            return notification;
        }
    }
}