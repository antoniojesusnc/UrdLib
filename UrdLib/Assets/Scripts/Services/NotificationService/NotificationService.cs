using System;
using Unity.Notifications;
using UnityEngine;
using UnityEngine.Serialization;
using Urd.Notifications;

namespace Urd.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private const string MAIN_CHANNEL = "Main Channel";
        
        [SerializeField]
        private NotificationsConfig _notificationsConfig;

        public override int LoadPriority => 90;

        private IUnityService _unityService;

        public void SetConfig(NotificationsConfig notificationsConfig)
        {
            _notificationsConfig = notificationsConfig;
        }

        public override void Init()
        {
            base.Init();

            _unityService = StaticServiceLocator.Get<IUnityService>();
            _unityService.OnGamePaused += OnGamePaused;
            var notificationArgs = new NotificationCenterArgs();
            notificationArgs.AndroidChannelId = MAIN_CHANNEL;
            NotificationCenter.Initialize(notificationArgs);
            NotificationCenter.RequestPermission();
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
            if (_notificationsConfig == null) 
            {
                return;
            }

            for (int i = 0; i < _notificationsConfig.Notifications.Count; i++)
            {
                if (_notificationsConfig.Notifications[i].CanShowNotification())
                {
                    TryScheduleNotification(_notificationsConfig.Notifications[i]);
                }
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