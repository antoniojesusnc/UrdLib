using Urd.Ads;
using Urd.Services.EventBus;

namespace Urd.Events
{
    public class OnBannerLoadedEvent : IEventBusMessage
    {
        public float HeightInPixels { get; private set; }
        public AdMobBannerError Error { get; private set; }

        public OnBannerLoadedEvent(float heightInPixels, AdMobBannerError error)
        {
            HeightInPixels = heightInPixels;
            Error = error;
        }
    }
}
