using System;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationFade", menuName = "Urd/Services/DotweenAnimations/TweenAnimationFade", order = 1)]
    public class TweenAnimationFade : TweenAnimation<PopupDotweenAnimationTypes>, ITweenAnimation<CanvasGroup>
    {
        [Header("Specific Configs")]
        [SerializeField] private float _initialFade;
        [SerializeField] private float _finalFade;

        private void Awake()
        {
            _duration = 0.2f;
            _initialFade = 0;
            _finalFade = 1;
        }

        public Tween DoAnimation(CanvasGroup rectTransform)
        {
            if (rectTransform == null)
            {
                return null;
            }

            rectTransform.alpha = _initialFade;
            return rectTransform.DOFade(_finalFade, _duration);
        }
    }
}
