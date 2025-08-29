using Urd.Services.EventBus;

namespace Urd
{
    public class OnAppOpenThroughANotificationEvent : IEventBusMessage
    {
        public string NotificationTitle {get; private set;}
        public OnAppOpenThroughANotificationEvent(string notificationTitle)
        {
            NotificationTitle = notificationTitle;
        }
    }
}
