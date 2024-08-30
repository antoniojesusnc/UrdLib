using System;
using UnityEngine;

namespace Urd.Services
{
    public interface IAdsService : IBaseService
    {
        void SetProvider(IAdsServiceProvider provider);
        public void ShowBanner(AdsBannerModel adsBannerModel);
        public void HideBanner() ;
        public void ShowInterstitial(Action<bool> onInterstitialWatchedCallback) ;
        public void HideInterstitial() ;
        public void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback) ;
        public void HideRewardedVideo() ;
    }
}