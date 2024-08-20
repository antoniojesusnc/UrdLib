using System;

namespace Urd.Services
{
    public interface IAdsServiceProvider
    {
        void Init(Action onInitCallback = null);
        void ShowBanner(AdsBannerModel adsBannerModel);
        void HideBanner();
        void ShowInterstitial();
        void HideInterstitial();
        void ShowRewardedVideo();
        void HideRewardedVideo();
    }
}