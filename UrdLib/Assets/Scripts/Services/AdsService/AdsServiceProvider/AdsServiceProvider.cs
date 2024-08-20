using System;
using UnityEngine;

namespace Urd.Services
{
    public abstract class AdsServiceProvider : IAdsServiceProvider
    {
        [SerializeField]
        protected string _androidBannerId;
        [SerializeField]
        protected string _iosBannerId;
        
        public virtual void Init(Action onInitializeCallback) { }
        public virtual void Dispose() { }

        public abstract void ShowBanner(AdsBannerModel adsBannerModel);

        public abstract void HideBanner();
        public abstract void ShowInterstitial();
        public abstract void HideInterstitial();
        public abstract void ShowRewardedVideo();
        public abstract void HideRewardedVideo();
        
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