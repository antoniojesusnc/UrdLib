using System;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Urd.Animation;
using Urd.Animations;
using Urd.Services;

namespace Urd.Navigation
{
    public abstract class UIBoomerangView : MonoBehaviour
    {
        [field: SerializeField]
        public abstract Enum Type { get; }
        
        public UIBoomerangModel Model { get; private set; }

        [Header("Components")]
        private CanvasGroup _background;
        private CanvasGroup _container;

        [Header("Animations")] 
        [SerializeField]
        private bool _useAnimationsWhenOpenOrClose;
        [SerializeField, ConditionalField("_useAnimationsWhenOpenOrClose")] 
        protected TweenAnimation _openAnimation;
        [SerializeField, ConditionalField("_useAnimationsWhenOpenOrClose")] 
        protected TweenAnimation _closeAnimation;

        protected List<Tween> _activeTween = new List<Tween>();
        
        private IDotweenAnimationService _dotweenAnimationService;
        
        protected virtual void Awake()
        {
            var backgroundTransform = transform.Find("BlackBackground");
            if (backgroundTransform != null)
            {
                _background = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            var containerTransform = transform.Find("Container");
            if (containerTransform != null)
            {
                _container = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            _background.alpha = 0;
            _container.alpha = 0;
        }

        public virtual void Init(UIBoomerangModel model)
        {
            Model = model;
            
            _dotweenAnimationService = StaticServiceLocator.Get<IDotweenAnimationService>();

            SetHeaderText();
        }

        private void SetHeaderText()
        {
            //_headerText.text = Model.Type.ToString();
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
                    tweenDialog = fadeAnimation.DoAnimation(_container);
                }
                else
                {
                    _container.alpha = 1;
                }
            }
            else
            {
                tweenDialog = SetUpOpenAnimation();
            }
            
            if (tweenBlackground != null || tweenDialog != null)
            {
                if((tweenBlackground?.Delay() + tweenBlackground?.Duration() ?? 0) > (tweenDialog?.Delay() + tweenDialog?.Duration() ?? 0))
                {
                    tweenBlackground.onComplete += () => OnOpen(onOpenCallback);
                    tweenBlackground.onComplete += () => _activeTween.Remove(tweenBlackground);
                    _activeTween.Add(tweenBlackground);
                }
                else
                {
                    tweenDialog.onComplete += () => OnOpen(onOpenCallback);
                    tweenDialog.onComplete += () => _activeTween.Remove(tweenDialog);
                    _activeTween.Add(tweenDialog);
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

        public void CloseBoomerang()
        {
            StaticServiceLocator.Get<INavigationService>().Close(Model);
        }
        
        public virtual void Close(Action onCloseCallback = null)
        {
            Tween tweenBlackground = null;
            Tween tweenDialog = null;
            if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(PopupDotweenAnimationTypes.FadeOut, out var fadeAnimation))
            {
                tweenBlackground = fadeAnimation.DoAnimation(_background);
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
                    tweenDialog = fadeAnimation.DoAnimation(_container);
                }
                else
                {
                    _container.alpha = 0;
                }
            }
            else
            {
                tweenDialog = SetUpCloseAnimation();
            }

            if (tweenBlackground != null || tweenDialog != null)
            {
                if((tweenBlackground?.Delay() + tweenBlackground?.Duration() ?? 0) > (tweenDialog?.Delay() + tweenDialog?.Duration() ?? 0))
                {
                    tweenBlackground.onComplete += () => OnClose(onCloseCallback);
                    tweenBlackground.onComplete += () => _activeTween.Remove(tweenBlackground);
                    _activeTween.Add(tweenBlackground);
                }
                else
                {
                    tweenDialog.onComplete += () => OnClose(onCloseCallback);
                    tweenDialog.onComplete += () => _activeTween.Remove(tweenDialog);
                    _activeTween.Add(tweenDialog);
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

        private void OnFinishBoomerangMovement(Action onCloseCallback)
        {
            _container.alpha = 0;
            OnClose(onCloseCallback);
        }

        protected virtual void OnClose(Action onCloseCallback)
        {
            onCloseCallback?.Invoke();
        }
    }
}
