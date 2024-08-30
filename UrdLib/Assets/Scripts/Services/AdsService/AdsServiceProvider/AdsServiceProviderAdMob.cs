using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class AdsServiceProviderAdMob : AdsServiceProvider
    {
        private const string ADMOB_CONFIG_FILE_PATH = "GoogleMobileAdsSettings"; 
        
        private BannerView _banner;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedVideo;
        
        public bool IsInitialized { get; private set; }
        public override void Init(Action onInitializeCallback)
        {
            base.Init(onInitializeCallback);
            
            MobileAds.Initialize(
                status => OnInitialize(status, onInitializeCallback));
        }

        private void OnInitialize(InitializationStatus status, Action onInitializeCallback)
        {
            IsInitialized = true;
            onInitializeCallback?.Invoke();
            
            LoadRewardVideo();
        }

        public override void ShowBanner(AdsBannerModel adsBannerModel)
        {
            if (_banner?.IsDestroyed == false)
            {
                HideBanner();
            }
            
            _banner = new BannerView(GetBannerAdUnitId(),
                                     //AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(adsBannerModel.Size.y),
                                     new AdSize(adsBannerModel.Size.x, adsBannerModel.Size.y),
                                     GetAdsPosition(adsBannerModel));


            var request = new AdRequest();
            _banner.LoadAd(request);
        }

        public override void HideBanner()
        {
            _banner?.Destroy();
        }

        public override void ShowInterstitial(Action<bool> onInterstitialWatchedCallback)
        {
            var request = new AdRequest();
            InterstitialAd.Load(GetBannerAdUnitId(), request, (interstitialAd, loadAdError) => OnInterstitialLoaded(interstitialAd, loadAdError, onInterstitialWatchedCallback) );
        }

        private void OnInterstitialLoaded(InterstitialAd interstitial, LoadAdError loadAdError,
            Action<bool> onInterstitialWatchedCallback)
        {
            _interstitialAd = interstitial;
            if (_interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
                _interstitialAd.OnAdFullScreenContentFailed +=
                    (error) => onInterstitialWatchedCallback?.Invoke(false);
                _interstitialAd.OnAdFullScreenContentClosed +=
                    () => onInterstitialWatchedCallback?.Invoke(true);
            }
        }

        public override void HideInterstitial()
        {
            _interstitialAd.Destroy();
        }

        public override void ShowRewardedVideo(Action<bool> onRewardVideoWatchedCallback)
        {
            if (_rewardedVideo != null && _rewardedVideo.CanShowAd())
            {
                ShowRewardedVideoInternal(onRewardVideoWatchedCallback);
            }
            else
            {
                LoadRewardVideo(true, onRewardVideoWatchedCallback);
            }
        }

        private void LoadRewardVideo(bool showAfterLoad = false, Action<bool> onRewardVideoWatchedCallback = null)
        {
            var request = new AdRequest();
            RewardedAd.Load(GetRVAdUnitId(), request, (rewardedVideo, loadAdError) => OnRewardedVideoLoaded(rewardedVideo, loadAdError, showAfterLoad,  onRewardVideoWatchedCallback));
        }

        private void OnRewardedVideoLoaded(RewardedAd rewardedVideo, LoadAdError loadAdError,
            bool showAfterLoad, Action<bool> onRewardVideoWatchedCallback)
        {
            _rewardedVideo = rewardedVideo;
            if (!showAfterLoad)
            {
                return;
            }

            if (_rewardedVideo.CanShowAd())
            {
                ShowRewardedVideoInternal(onRewardVideoWatchedCallback);
            }
            else
            {
                onRewardVideoWatchedCallback?.Invoke(false);
            }
        }

        private void ShowRewardedVideoInternal(Action<bool> onRewardVideoWatchedCallback)
        {
            var reward = _rewardedVideo.GetRewardItem();
            reward.Type = "temp";
            reward.Amount = 11;
            _rewardedVideo.Show(null);
            _rewardedVideo.OnAdFullScreenContentClosed += () => OnCloseRewardVideo(true, onRewardVideoWatchedCallback);
            _rewardedVideo.OnAdFullScreenContentFailed += (error) => OnCloseRewardVideo(false, onRewardVideoWatchedCallback);
        }

        private void OnCloseRewardVideo(bool success, Action<bool> onRewardVideoWatchedCallback)
        {
            _rewardedVideo?.Destroy();
            _rewardedVideo = null;
            onRewardVideoWatchedCallback?.Invoke(success);
        }

        public override void HideRewardedVideo()
        {
            _rewardedVideo?.Destroy();
        }

        private AdPosition GetAdsPosition(AdsBannerModel adsBannerModel)
        {
            switch (adsBannerModel.Position)
            {
                case AdsBannerPosition.Top: return AdPosition.Top;
                case AdsBannerPosition.Bottom:return AdPosition.Bottom;
                case AdsBannerPosition.TopLeft:return AdPosition.TopLeft;
                case AdsBannerPosition.TopRight:return AdPosition.TopRight;
                case AdsBannerPosition.BottomLeft:return AdPosition.BottomLeft;
                case AdsBannerPosition.BottomRight:return AdPosition.BottomRight;
                case AdsBannerPosition.Center:return AdPosition.Center;
                default: return AdPosition.Top;
            }
        }
    }
}