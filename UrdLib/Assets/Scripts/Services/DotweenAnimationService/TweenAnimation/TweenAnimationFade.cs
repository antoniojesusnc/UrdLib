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
        

        public Tween DoAnimation(CanvasGroup rectTransform)
        {
            rectTransform.alpha = _initialFade;
            return rectTransform.DOFade(_finalFade, _duration);
        }
    }
}
