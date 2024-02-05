using System;
using DG.Tweening;
using UnityEngine;
using Urd.Animation;
using Urd.Services;

namespace Urd.Navigation
{
    public class UIPopupView : MonoBehaviour
    {
        [field: SerializeField]
        public UIPopupTypes PopupType { get; private set; }

        public UIPopupModel Model { get; private set; }

        [Header("Components")]
        private CanvasGroup _background;
        private CanvasGroup _container;

        [Header("Animations")]                              
        [SerializeField] private TweenAnimation _openAnimation;
        [SerializeField] private TweenAnimation _closeAnimation;
        
        private IDotweenAnimationService _dotweenAnimationService;
        
        protected virtual void Awake()
        {
            var backgroundTransform = transform.GetChild(0).Find("BlackBackground");
            if (backgroundTransform != null)
            {
                _background = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            var containerTransform = transform.GetChild(0).Find("Dialog");
            if (containerTransform != null)
            {
                _container = backgroundTransform.GetComponent<CanvasGroup>();
            }
            
            _background.alpha = 0;
            _container.alpha = 0;
        }

        public virtual void Init(UIPopupModel model)
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
            Tween tween = null;
            if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(
                    PopupDotweenAnimationTypes.BackgroundFadeIn, out var fadeAnimation))
            {
                //tween = fadeAnimation.DoAnimation(_background);
            }

            if (_openAnimation != null)
            {
                var tweenAnimation = (_openAnimation as ITweenAnimation<RectTransform>);
                tween = tweenAnimation?.DoAnimation(_container.GetComponent<RectTransform>());
            }
            else if (_dotweenAnimationService.TryGetAnimation<TweenAnimationPopupMoveAnchorFooter>(
                         PopupDotweenAnimationTypes.PopupShowFromFooter, out var showAnimation))
            {
                tween = showAnimation.DoAnimation(_container.GetComponent<RectTransform>());
            }

            _container.alpha = 1;
            if (tween != null)
            {
                tween.onComplete += () => OnOpen(onOpenCallback);
            }
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
            Tween tween = null;
            if (_dotweenAnimationService.TryGetAnimation<TweenAnimationFade>(PopupDotweenAnimationTypes.BackgroundFadeOut, out var fadeAnimation))
            {
                //tween = fadeAnimation.DoAnimation(_background);
            }
            
            if (_closeAnimation != null)
            {
                var tweenAnimation = (_closeAnimation as ITweenAnimation<RectTransform>);
                tween = tweenAnimation?.DoAnimation(_container.GetComponent<RectTransform>());
            }
            else if (_dotweenAnimationService.TryGetAnimation<TweenAnimationPopupMoveAnchorFooter>(
                           PopupDotweenAnimationTypes.DialogHideFromFooter, out var hideAnimation))
            {
                tween = hideAnimation.DoAnimation(_container.GetComponent<RectTransform>());
            }

            if (tween != null)
            {
                tween.onComplete += () => OnFinishPopupMovement(onCloseCallback);
            }
        }

        private void OnFinishPopupMovement(Action onCloseCallback)
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
