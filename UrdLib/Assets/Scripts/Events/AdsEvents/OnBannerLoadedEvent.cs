using Urd.Services.EventBus;

namespace Urd.Events
{
    public class OnBannerLoadedEvent : IEventBusMessage
    {
        public bool LoadedSuccess { get; private set; }

        public OnBannerLoadedEvent(bool loadedSuccess)
        {
            LoadedSuccess = loadedSuccess;
        }
    }
}
