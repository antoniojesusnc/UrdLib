#if UNITY_ANDROID || UNITY_IOS
using System;
using UnityEngine;
using Urd.Notifications;

using Unity.Notifications;
using Unity.Notifications.Android;
using UnityEngine.Android;

namespace Urd.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private const string MAIN_CHANNEL = "Main Channel";

        [SerializeField] private NotificationsConfig _notificationsConfig;

        public override int LoadPriority => 90;

        private IUnityService _unityService;

        private bool _hasPermission;

        public void SetConfig(NotificationsConfig notificationsConfig)
        {
            _notificationsConfig = notificationsConfig;
        }

        public override void Init()
        {
            base.Init();
        }

        private void InitNotifications()
        {
            var permissionStatus = AndroidNotificationCenter.UserPermissionToPost;
            if (permissionStatus == PermissionStatus.DeniedDontAskAgain)
            {
                _hasPermission = false;
                return;
            }
            
            if (permissionStatus == PermissionStatus.Allowed)
            {
                _hasPermission = true;
                return;
            }
            
            if (permissionStatus == PermissionStatus.NotRequested
                     || permissionStatus == PermissionStatus.RequestPending
                     || permissionStatus == PermissionStatus.Denied)
            {
                _unityService = StaticServiceLocator.Get<IUnityService>();
                _unityService.OnGamePaused += OnGamePaused;
                var notificationArgs = new NotificationCenterArgs();
                notificationArgs.AndroidChannelId = MAIN_CHANNEL;
                notificationArgs.AndroidChannelName = MAIN_CHANNEL;
                notificationArgs.AndroidChannelDescription = MAIN_CHANNEL;
                NotificationCenter.Initialize(notificationArgs);
                _hasPermission = AndroidNotificationCenter.Initialize();
            }
        }

        public void RequestPermission()
        {
            try
            {
                InitNotifications();
                if (!_hasPermission)
                {
                    NotificationCenter.RequestPermission();
                    InitNotifications();
                }
            }
            catch
            {
                _hasPermission = false;
            }
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
            if (_notificationsConfig == null || !_hasPermission)
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
            if (!_hasPermission)
            {
                return;
            }
            
            NotificationCenter.CancelAllDeliveredNotifications();
            NotificationCenter.CancelAllScheduledNotifications();
        }
    }
}
#endif
