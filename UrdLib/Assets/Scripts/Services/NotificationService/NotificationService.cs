using System;
using Unity.Notifications;
using UnityEngine;

namespace Urd.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        [SerializeField]
        private NotificationServiceConfig _notificationServiceConfig;

        public override int LoadPriority => 90;

        private IUnityService _unityService;

        public override void Init()
        {
            base.Init();

            _unityService = StaticServiceLocator.Get<IUnityService>();

            _unityService.OnGamePaused += OnGamePaused;
        }

        private void OnGamePaused(bool paused)
        {
            if (paused)
            {
                ScheduleNotifications();
            }
            else
            {
                CancelNotifications();
            }
        }

        private void ScheduleNotifications()
        {
            if (_notificationServiceConfig == null) 
            {
                return;
            }

            for (int i = 0; i < _notificationServiceConfig.Notifications.Count; i++)
            {
                TryScheduleNotification(_notificationServiceConfig.Notifications[i]);
            }
        }

        private void TryScheduleNotification(INotificationModel notificationModel)
        {
            NotificationCenter.ScheduleNotification(notificationModel.GetNotification(), new NotificationDateTimeSchedule(notificationModel.DeliveryDateTime));
        }

        public void CancelNotifications()
        {
            NotificationCenter.CancelAllDeliveredNotifications();
            NotificationCenter.CancelAllScheduledNotifications();
        }
    }
}