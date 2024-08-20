using UnityEngine;

namespace Urd.Services
{
    [System.Serializable]
    public class AdsService : BaseService, IAdsService
    {
        public override int LoadPriority => 10;
        
        [SerializeReference, SubclassSelector]
        private IAdsServiceProvider _adsServiceProvider;
        
        public override void Init()
        {
            SetProvider(_adsServiceProvider);
        }
        
        public void SetProvider(IAdsServiceProvider provider)
        {
            _adsServiceProvider = provider;
            _adsServiceProvider.Init();
        }

        public void ShowBanner(AdsBannerModel adsBannerModel) => _adsServiceProvider.ShowBanner(adsBannerModel);
        public void HideBanner() => _adsServiceProvider.HideBanner();
        public void ShowInterstitial() => _adsServiceProvider.ShowInterstitial();
        public void HideInterstitial() => _adsServiceProvider.HideInterstitial();
        public void ShowRewardedVideo() => _adsServiceProvider.ShowRewardedVideo();
        public void HideRewardedVideo() => _adsServiceProvider.HideRewardedVideo();
    }
}