using Urd.Services.EventBus;

namespace Urd.Events
{
    public class OnRewardedVideoWatchedEvent : IEventBusMessage
    {
        public bool WatchedSuccess { get; private set; }
        
        public OnRewardedVideoWatchedEvent(bool watchedSuccess)
        {
            WatchedSuccess = watchedSuccess;
        }
    }
}
