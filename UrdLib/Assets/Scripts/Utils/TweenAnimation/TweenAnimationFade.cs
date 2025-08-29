using System;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationFade", menuName = "Urd/Services/DotweenAnimations/TweenAnimationFade", order = 1)]
    public class TweenAnimationFade : TweenAnimation, ITweenAnimation<CanvasGroup>
    {
        [Header("Specific Configs")]
        [SerializeField] private float _initialFade;
        [SerializeField] private float _finalFade;

        private void Awake()
        {
            Duration = 0.2f;
            _initialFade = 0;
            _finalFade = 1;
        }

        public Tween DoAnimation(CanvasGroup canvasGroup)
        {
            if (canvasGroup == null)
            {
                return null;
            }

            canvasGroup.alpha = _initialFade;
            return canvasGroup.DOFade(_finalFade, Duration);
        }

    }
}
