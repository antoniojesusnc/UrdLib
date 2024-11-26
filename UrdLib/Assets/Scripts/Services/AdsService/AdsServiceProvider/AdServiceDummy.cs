using System;
using Urd.Ads;
using Urd.Services;

namespace RubberDuck
{
    [Serializable]
    public class AdServiceDummy : IAdsServiceProvider
    {
        public float BannerSize => 0;

        public void Init(Action onInitCallback = null) {
            
        }

        public void ShowBanner(AdsBannerModel adsBannerModel, Action<AdMobBannerError> onBannerLoaded)
        {
            var bannerError = new AdMobBannerError();
            bannerError.SetAsError();
            onBannerLoaded?.Invoke(bannerError);
        }

        public void HideBanner()
        {
        }

        public void ShowInterstitial(Action<bool> onRewardVideoWatchedCallback)
        {
            onRewardVideoWatchedCallback?.Invoke(false);
        }

        public void HideInterstitial()
        {
        }

        public void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback)
        {
            onRewardVideoWatchedCallback?.Invoke(false);
        }

        public void HideRewardedVideo()
        {
        }

        public bool CanShowRewardedVideo(bool loadIfCannot)
        {
            return false;
        }
    }
}