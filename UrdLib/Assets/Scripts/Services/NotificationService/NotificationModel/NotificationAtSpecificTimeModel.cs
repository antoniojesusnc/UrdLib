using System;
using Unity.Notifications;
using UnityEngine;
using UnityEngine.Localization;

namespace Urd.Notifications
{
    [Serializable]
    public class NotificationAtSpecificTimeModel : NotificationModel
    {
        [SerializeField]
        private int _hour24HoursFormat;
        [SerializeField]
        private int _minute;

        public NotificationAtSpecificTimeModel()
        {
            
        }
        
        protected override DateTime GetDeliveryTime()
        {
            var now = DateTime.UtcNow;
            if (now.Hour > _hour24HoursFormat || now.Minute > _minute)
            {
                now = now.AddDays(1);
            }
            return new DateTime(now.Year, now.Month, now.Day, _hour24HoursFormat, _minute, 0);
        }

        public override bool CanShowNotification()
        {
            return true;
        }
    }
}