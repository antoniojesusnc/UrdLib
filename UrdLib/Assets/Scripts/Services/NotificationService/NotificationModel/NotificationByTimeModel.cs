using System;
using Unity.Notifications;
using UnityEngine;
using UnityEngine.Localization;

namespace Urd.Notifications
{
    [Serializable]
    public class NotificationByTimeModel : NotificationModel
    {
        [SerializeField]
        private float _delayTimeInMinutes;

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

        protected override DateTime GetDeliveryTime()
        {
            return DateTime.UtcNow.AddMinutes(_delayTimeInMinutes);
        }

        public override bool CanShowNotification()
        {
            return true;
        }
    }
}