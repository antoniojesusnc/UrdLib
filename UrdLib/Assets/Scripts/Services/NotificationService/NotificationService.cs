#if UNITY_ANDROID || UNITY_IOS
using System;
using UnityEngine;
using Urd.Notifications;

using Unity.Notifications;
using UnityEngine.Android;

namespace Urd.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private const string NOTIFICATION_PERMISION = "NOTIFICATION_PERMISION";

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

            _hasPermission = StaticServiceLocator.Get<ISaveLoadService>().Load(NOTIFICATION_PERMISION, true);

            if (_hasPermission)
            {
                _unityService = StaticServiceLocator.Get<IUnityService>();
                _unityService.OnGamePaused += OnGamePaused;
                var notificationArgs = new NotificationCenterArgs();
                notificationArgs.AndroidChannelId = MAIN_CHANNEL;
                NotificationCenter.Initialize(notificationArgs);
            }
        }

        public void RequestPermission()
        {
            if (!_hasPermission)
            {
                return;
            }

#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                PermissionCallbacks permissionCallback = new PermissionCallbacks();
                permissionCallback.PermissionGranted += OnPermissionGranted;
                permissionCallback.PermissionDenied += OnPermissionDenied;
                permissionCallback.PermissionDeniedAndDontAskAgain += OnPermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", permissionCallback);
            }
            else
            {
                _hasPermission = true;
            }

#else
                NotificationCenter.RequestPermission();
#endif
        }

        private void OnPermissionDeniedAndDontAskAgain(string obj)
        {
            _hasPermission = false;
            StaticServiceLocator.Get<ISaveLoadService>().Save(NOTIFICATION_PERMISION, false);
        }


        private void OnPermissionDenied(string obj)
        {
            _hasPermission = false;
        }

        private void OnPermissionGranted(string obj)
        {
            _hasPermission = true;
            StaticServiceLocator.Get<ISaveLoadService>().Save(NOTIFICATION_PERMISION, true);
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
