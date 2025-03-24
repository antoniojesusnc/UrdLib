using System;

namespace Urd.Ads
{
    public interface IAdsServiceProvider
    {
        float BannerSize { get; }
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