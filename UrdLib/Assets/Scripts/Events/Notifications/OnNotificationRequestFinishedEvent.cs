using Urd.Services.EventBus;
using Unity.Notifications;

namespace Urd
{
    public class OnNotificationRequestFinishedEvent : IEventBusMessage
    {
        public NotificationsPermissionStatus RequestStatus { get; private set; }
        public OnNotificationRequestFinishedEvent(NotificationsPermissionStatus requestStatus)
        {
            RequestStatus = requestStatus;
        }
    }
}
