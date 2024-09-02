using System;
using DG.Tweening;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;
using Urd.Events;

namespace Urd.Services
{
    [Serializable]
    public class AdsServiceProviderAdMob : AdsServiceProvider
    {
        private const string ADMOB_CONFIG_FILE_PATH = "GoogleMobileAdsSettings"; 
        
        private BannerView _banner;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedVideo;
        private IEventBusService _eventBusService;

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

            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            LoadRewardVideo();
        }

        public override void ShowBanner(AdsBannerModel adsBannerModel, Action<bool> onBannerLoaded)
        {
            if (_banner?.IsDestroyed == false)
            {
                HideBanner();
            }

            var adSize = adsBannerModel.Size.x == 0
                ? AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(adsBannerModel.Size.y)
                : new AdSize(adsBannerModel.Size.x, adsBannerModel.Size.y);
            
            _banner = new BannerView(GetBannerAdUnitId(),adSize, GetAdsPosition(adsBannerModel));

            var request = new AdRequest();
            _banner.OnBannerAdLoaded += () => OnBannerLoaded(null, onBannerLoaded);
            _banner.OnBannerAdLoadFailed += (error) => OnBannerLoaded(error, onBannerLoaded);
            _banner.LoadAd(request);
            Debug.Log("Loading Banner");
        }

        private void OnBannerLoaded(LoadAdError error, Action<bool> onBannerLoaded)
        {
            if (error == null)
            {
                Debug.Log($"Banner loaded success");
            }
            else
            {
                Debug.Log($"Banner loaded with error: {error.GetResponseInfo()}");
            }
            _eventBusService.Send(new OnBannerLoadedEvent(error == null));
            onBannerLoaded?.Invoke(error == null);
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

        public override bool CanShowRewardedVideo(bool loadIfCannot)
        {
            bool canShowVideo =  _rewardedVideo?.CanShowAd() ?? false;
            if (!canShowVideo && loadIfCannot)
            {
                LoadRewardVideo();
            }

            return canShowVideo;
        }

        private void LoadRewardVideo(bool showAfterLoad = false, Action<bool> onRewardVideoWatchedCallback = null)
        {
            var request = new AdRequest();
            RewardedAd.Load(GetRVAdUnitId(), request, (rewardedVideo, loadAdError) => OnRewardedVideoLoaded(rewardedVideo, loadAdError, showAfterLoad,  onRewardVideoWatchedCallback));
        }

        private void OnRewardedVideoLoaded(RewardedAd rewardedVideo, LoadAdError loadAdError,
            bool showAfterLoad, Action<bool> onRewardVideoWatchedCallback)
        {
            _eventBusService.Send(new OnRewardedVideoLoadedEvent(loadAdError == null));
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
            _rewardedVideo.OnAdFullScreenContentClosed += () => OnCloseRewardVideo(true, onRewardVideoWatchedCallback);
            _rewardedVideo.OnAdFullScreenContentFailed += (error) => OnCloseRewardVideo(false, onRewardVideoWatchedCallback);
            _rewardedVideo.Show(null);
        }

        private void OnCloseRewardVideo(bool success, Action<bool> onRewardVideoWatchedCallback)
        {
            _eventBusService.Send(new OnRewardedVideoWatchedEvent(success));
            _rewardedVideo?.Destroy();
            _rewardedVideo = null;
            onRewardVideoWatchedCallback?.Invoke(success);
        }

        public override void HideRewardedVideo()
        {
            _rewardedVideo?.Destroy();
            _rewardedVideo = null;
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