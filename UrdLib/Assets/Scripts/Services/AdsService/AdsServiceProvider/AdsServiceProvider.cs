using System;
using UnityEngine;
using Urd.Ads;

namespace Urd.Services
{
    public abstract class AdsServiceProvider : IAdsServiceProvider
    {
        [Header("Banner")]
        [SerializeField]
        protected string _androidBannerId;
        [SerializeField]
        protected string _iosBannerId;
        
        [Header("Reward Video")]
        [SerializeField]
        protected string _androidRVId;
        [SerializeField]
        protected string _iosRVId;
        
        public virtual void Init(Action onInitializeCallback) { }
        public virtual void Dispose() { }

        public abstract void ShowBanner(AdsBannerModel adsBannerModel, Action<AdMobBannerError> onBannerLoaded);

        public abstract void HideBanner();
        public abstract void ShowInterstitial(Action<bool> onRewardVideoWatchedCallback);
        public abstract void HideInterstitial();
        public abstract void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback);

        public abstract bool CanShowRewardedVideo(bool loadIfCannot);
        public abstract void HideRewardedVideo();

        protected string GetRVAdUnitId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android: return _androidRVId;                 
                case RuntimePlatform.IPhonePlayer: return _iosRVId;
                default: return _androidBannerId;
            }
        }
        protected string GetBannerAdUnitId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android: return _androidBannerId;                 
                case RuntimePlatform.IPhonePlayer: return _iosBannerId;
                default: return _androidBannerId;
            }
        }
    }
}