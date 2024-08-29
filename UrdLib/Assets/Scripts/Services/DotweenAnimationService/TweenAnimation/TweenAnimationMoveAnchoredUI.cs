using DG.Tweening;

using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationMoveAnchoredUI", menuName = "Urd/Services/DotweenAnimations/TweenAnimationMoveAnchoredUI", order = 1)]
    public class TweenAnimationMoveAnchoredUI : TweenAnimation<PopupDotweenAnimationTypes>, ITweenAnimation<RectTransform>
    {
        [Header("Specific Configs"), SerializeField] private Vector2 _finalPositionOffset;

        public Tween DoAnimation(RectTransform rectTransform)
        {
            return rectTransform.DOAnchorPos(rectTransform.anchoredPosition + 
                                             rectTransform.lossyScale.x*_finalPositionOffset, _duration)
                                .SetEase(_ease);
        }
    }
}
