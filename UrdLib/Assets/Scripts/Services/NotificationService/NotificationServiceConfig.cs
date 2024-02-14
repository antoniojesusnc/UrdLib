using System.Collections.Generic;
using UnityEngine;
using Urd.Notifications;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "NotificationServiceConfig", menuName = "Urd/Services/Notifications Service Config", order = 1)]

    public class NotificationServiceConfig : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<INotificationModel> Notifications { get; private set; }

        private void Awake()
        {
            CreateNotificationExample();
        }

        [ContextMenu("Crate Notification Example")]
        private void CreateNotificationExample()
        {
            Notifications.Add(new NotificationByTimeModel(5, "Test Notification"));
        }
    }
}