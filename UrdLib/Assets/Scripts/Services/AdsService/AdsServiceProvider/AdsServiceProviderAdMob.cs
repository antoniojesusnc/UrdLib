using System;
using DG.Tweening;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using Urd.Ads;
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

        public override void ShowBanner(AdsBannerModel adsBannerModel, Action<AdMobBannerError> onBannerLoaded)
        {
            if (_banner?.IsDestroyed == false)
            {
                HideBanner();
            }

            var adSize = adsBannerModel.Size.x <= 0
                ? AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth)
                : new AdSize(adsBannerModel.Size.x, adsBannerModel.Size.y);

            Debug.Log($"loading Banner of size:({adSize.Width},{adSize.Height}");
            _banner = new BannerView(GetBannerAdUnitId(),adSize, GetAdsPosition(adsBannerModel));

            var request = new AdRequest();
            _banner.OnBannerAdLoaded += () => OnBannerLoaded(null, onBannerLoaded);
            _banner.OnBannerAdLoadFailed += (error) => OnBannerLoaded(error, onBannerLoaded);
            Debug.Log("Loading Banner");
            _banner.LoadAd(request);
        }

        private void OnBannerLoaded(LoadAdError error, Action<AdMobBannerError> onBannerLoaded)
        {
            AdMobBannerError bannerError = new AdMobBannerError();
            if (error == null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                                                 Debug.Log($"Banner loaded success"));
            }
            else
            {
                UnityMainThreadDispatcher.Instance().Enqueue(()=> 
                    Debug.Log($"Banner loaded with error: {error}"));
                bannerError = JsonConvert.DeserializeObject<AdMobBannerError>(error.ToString());
            }
            
            var scale = MobileAds.Utils.GetDeviceScale();
            if (scale == 0)
            {
                scale = 1;
            }
            DOVirtual.DelayedCall(0.1f, () => 
                _eventBusService.Send(new OnBannerLoadedEvent(_banner.GetHeightInPixels()/scale, bannerError)));
            onBannerLoaded?.Invoke(bannerError);
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
            _rewardedVideo = rewardedVideo;
            _eventBusService.Send(new OnRewardedVideoLoadedEvent(loadAdError == null));
            if (!showAfterLoad)
            {
                return;
            }

            ShowRewardedVideo(onRewardVideoWatchedCallback);
            
            /*
            if (_rewardedVideo.CanShowAd())
            {
                ShowRewardedVideoInternal(onRewardVideoWatchedCallback);
            }
            else
            {
                onRewardVideoWatchedCallback?.Invoke(false);
            }
            */
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
            _rewardedVideo?.Destroy();
            _rewardedVideo = null;

            DOVirtual.DelayedCall(
                0.1f, 
                () =>
                {
                    onRewardVideoWatchedCallback?.Invoke(success);
                    _eventBusService.Send(new OnRewardedVideoWatchedEvent(success));
                });
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