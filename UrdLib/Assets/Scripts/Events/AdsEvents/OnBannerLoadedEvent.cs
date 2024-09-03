using Urd.Services.EventBus;

namespace Urd.Events
{
    public class OnBannerLoadedEvent : IEventBusMessage
    {
        public float HeightInPixels { get; private set; }
        public bool LoadedSuccess { get; private set; }

        public OnBannerLoadedEvent(float heightInPixels, bool loadedSuccess)
        {
            HeightInPixels = heightInPixels;
            LoadedSuccess = loadedSuccess;
        }
    }
}
