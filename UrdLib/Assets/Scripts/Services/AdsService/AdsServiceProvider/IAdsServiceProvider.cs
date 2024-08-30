using System;

namespace Urd.Services
{
    public interface IAdsServiceProvider
    {
        void Init(Action onInitCallback = null);
        void ShowBanner(AdsBannerModel adsBannerModel);
        void HideBanner();
        void ShowInterstitial(Action<bool> onRewardVideoWatchedCallback);
        void HideInterstitial();
        void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback);
        void HideRewardedVideo();
    }
}