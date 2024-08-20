using UnityEngine;

namespace Urd.Services
{
    public interface IAdsService : IBaseService
    {
        void SetProvider(IAdsServiceProvider provider);
        public void ShowBanner(AdsBannerModel adsBannerModel);
        public void HideBanner() ;
        public void ShowInterstitial() ;
        public void HideInterstitial() ;
        public void ShowRewardedVideo() ;
        public void HideRewardedVideo() ;
    }
}