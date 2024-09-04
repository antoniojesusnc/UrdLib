using System;
using Urd.Ads;

namespace Urd.Services
{
    public interface IAdsService : IBaseService
    {
        void SetProvider(IAdsServiceProvider provider);
        void HideBanner();
        void ShowBanner(AdsBannerModel adsBannerModel, Action<AdMobBannerError> onBannerLoaded);
        public void ShowInterstitial(Action<bool> onInterstitialWatchedCallback) ;
        public void HideInterstitial() ;
        bool CanShowRewardedVideo(bool loadIfCannot = false);
        public void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback) ;
        public void HideRewardedVideo() ;
    }
}