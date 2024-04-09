using System;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Urd.Animation;
using Urd.Services;

namespace Urd.Navigation
{
    public abstract class UIPopupView : MonoBehaviour
    {
        [field: SerializeField]
        public abstract Enum Type { get; }

        public UIPopupModel Model { get; private set; }

        [Header("Components")]
        private CanvasGroup _background;
        private CanvasGroup _dialog;

        [Header("Animations")] 
        [SerializeField]
        private bool _useAnimationsWhenOpenOrClose;
        [SerializeField, ConditionalField("_useAnimationsWhenOpenOrClose")] 
        private TweenAnimation _openAnimation;
        [SerializeField, ConditionalField("_useAnimationsWhenOpenOrClose")] 
        private TweenAnimation _closeAnimation;
        
        private IDotweenAnimationService _dotweenAnimationService;
        private CanvasGroup _canvasGroup;
        
        protected virtual void Awake()
        {
            var backgroundTransform = transform.Find("BlackBackground");
            if (backgroundTransform != null)
            {
                _background = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            var containerTransform = transform.Find("Dialog");
            if (containerTransform != null)
            {
                _dialog = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            _background.alpha = 0;
            _dialog.alpha = 0;
        }

        public virtual void Init(UIPopupModel model)
        {
            Model = model;
            
            _dotweenAnimationService = StaticServiceLocator.Get<IDotweenAnimationService>();
        }

        public virtual void Open(Action onOpenCallback = null)
        {
            Tween tweenBlackground = null;
            Tween tweenDialog = null;
            if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(
                    PopupDotweenAnimationTypes.FadeIn, out var fadeAnimation))
            {
                tweenBlackground = fadeAnimation.DoAnimation(_background);
            }
            else
            {
                _background.alpha = 1;
            }

            if (!_useAnimationsWhenOpenOrClose)
            {
                if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(
                        PopupDotweenAnimationTypes.FadeIn, out fadeAnimation))
                {
                    tweenDialog = fadeAnimation.DoAnimation(_dialog);
                }
                else
                {
                    _dialog.alpha = 1;
                }
            }
            else
            {
                tweenDialog = SetUpOpenAnimation();
            }
            
            if (tweenBlackground != null || tweenDialog != null)
            {
                if((tweenBlackground?.Delay() + tweenBlackground?.Duration()) > (tweenDialog?.Delay() + tweenDialog?.Duration()))
                {
                    tweenBlackground.onComplete += () => OnOpen(onOpenCallback);
                }else
                {
                    tweenDialog.onComplete += () => OnOpen(onOpenCallback);
                }
            }
            else
            {
                OnOpen(onOpenCallback);
            }
        }

        protected virtual Tween SetUpOpenAnimation()
        {
            return default;
        }

        protected virtual void OnOpen(Action onOpenCallback)
        {
            onOpenCallback?.Invoke();
        }

        public void ClosePopup()
        {
            StaticServiceLocator.Get<INavigationService>().Close(Model);
        }
        
        public virtual void Close(Action onCloseCallback = null)
        {
            Tween tweenBlackground = null;
            Tween tweenDialog = null;
            if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(PopupDotweenAnimationTypes.FadeOut, out var fadeAnimation))
            {
                //tweenBlackground = fadeAnimation.DoAnimation(_background);
            }
            else
            {
                _background.alpha = 0;
            }
            
            if (!_useAnimationsWhenOpenOrClose)
            {
                if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(
                        PopupDotweenAnimationTypes.FadeOut, out fadeAnimation))
                {
                    //tweenDialog = fadeAnimation.DoAnimation(_dialog);
                }
                else
                {
                    _dialog.alpha = 0;
                }
            }
            else
            {
                tweenDialog = SetUpCloseAnimation();
            }

            if (tweenBlackground != null || tweenDialog != null)
            {
                if((tweenBlackground?.Delay() + tweenBlackground?.Duration()) > (tweenDialog?.Delay() + tweenDialog?.Duration()))
                {
                    tweenBlackground.onComplete += () => OnClose(onCloseCallback);
                }else
                {
                    tweenDialog.onComplete += () => OnClose(onCloseCallback);
                }
            }
            else
            {
                OnClose(onCloseCallback);
            }
        }

        protected virtual Tween SetUpCloseAnimation()
        {
            return default;
        }
        
        protected virtual void OnClose(Action onCloseCallback)
        {
            onCloseCallback?.Invoke();
        }
    }
}
