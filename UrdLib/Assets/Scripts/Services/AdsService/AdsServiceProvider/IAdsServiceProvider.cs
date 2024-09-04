using System;
using Urd.Ads;

namespace Urd.Services
{
    public interface IAdsServiceProvider
    {
        void Init(Action onInitCallback = null);
        void ShowBanner(AdsBannerModel adsBannerModel, Action<AdMobBannerError> onBannerLoaded);
        void HideBanner();
        void ShowInterstitial(Action<bool> onRewardVideoWatchedCallback);
        void HideInterstitial();
        void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback);
        void HideRewardedVideo();
        bool CanShowRewardedVideo(bool loadIfCannot);
    }
}