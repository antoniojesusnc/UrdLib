using System.Collections.Generic;
using UnityEngine;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "NotificationServiceConfig", menuName = "Urd/Services/Notifications Service Config", order = 1)]

    public class NotificationsConfig : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<INotificationModel> Notifications { get; private set; }
    }
}