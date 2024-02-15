using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Urd.Navigation;
using Urd.Notifications;
using Urd.Services;

namespace Urd.Editor
{
    [CustomEditor(typeof(NotificationsConfig))]
    public class NotificationConfigCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawAddExampleNotificationButton();
        }

        private void DrawAddExampleNotificationButton()
        {
            if (GUILayout.Button("Add Example Notification"))
            {
                var notification = target as NotificationsConfig;
                notification.Notifications.Add(new NotificationByTimeModel(5, "Test Notification"));

                AssetDatabase.SaveAssets();
            }
        }
    }
}
