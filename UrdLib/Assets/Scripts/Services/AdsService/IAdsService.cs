using System;
using UnityEngine;

namespace Urd.Services
{
    public interface IAdsService : IBaseService
    {
        void SetProvider(IAdsServiceProvider provider);
        void ShowBanner(AdsBannerModel adsBannerModel, Action<bool> onBannerLoaded);
        public void ShowInterstitial(Action<bool> onInterstitialWatchedCallback) ;
        public void HideInterstitial() ;
        bool CanShowRewardedVideo(bool loadIfCannot = false);
        public void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback) ;
        public void HideRewardedVideo() ;
    }
}