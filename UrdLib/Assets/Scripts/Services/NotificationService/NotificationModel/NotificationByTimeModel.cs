using System;
using Unity.Notifications;
using UnityEngine;
using UnityEngine.Localization;

namespace Urd.Notifications
{
    [Serializable]
    public class NotificationByTimeModel : INotificationModel
    {
        [SerializeField]
        private float _delayTimeInMinutes;

        [SerializeField]
        private LocalizedString _notificationText;

        public DateTime DeliveryDateTime => DateTime.UtcNow.AddMinutes(_delayTimeInMinutes);

        public NotificationByTimeModel()
        {
            
        }

        public NotificationByTimeModel(float delayTimeInMinutes, string key) : this(
            delayTimeInMinutes, new LocalizedString("Localizations", key)) { }
        
        public NotificationByTimeModel(float delayTimeInMinutes, LocalizedString message)
        {
            _delayTimeInMinutes = delayTimeInMinutes;
            _notificationText = message;
        }
        
        public Notification GetNotification()
        {
            var notification = new Notification();
            notification.Text = _notificationText.GetLocalizedString();

            return notification;
        }
    }
}