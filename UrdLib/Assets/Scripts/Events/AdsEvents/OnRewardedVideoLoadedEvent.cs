using Urd.Services.EventBus;

namespace Urd.Events
{
    public class OnRewardedVideoLoadedEvent : IEventBusMessage
    {
        public bool LoadedSuccess { get; private set; }

        public OnRewardedVideoLoadedEvent(bool loadedSuccess)
        {
            LoadedSuccess = loadedSuccess;
        }
    }
}
