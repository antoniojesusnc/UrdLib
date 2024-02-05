using DG.Tweening;

using UnityEngine;
using Urd.Animation;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationPopupMoveAnchorFooter", menuName = "Urd/Services/DotweenAnimations/TweenAnimationPopupMoveAnchorFooter", order = 1)]
    public class TweenAnimationPopupMoveAnchorFooter : TweenAnimation<PopupDotweenAnimationTypes>, ITweenAnimation<RectTransform>
    {
        public enum TweenAnimationPopupMoveAnchorFooterBehavior
        {
            Show,
            Hide
        }

        [Header("Specific Configs")]
        [SerializeField] private TweenAnimationPopupMoveAnchorFooterBehavior _behavior;

        public Tween DoAnimation(RectTransform rectTransform)
        {
            float finalYPosition = 0;
            
            if (_behavior == TweenAnimationPopupMoveAnchorFooterBehavior.Show)
            {
                rectTransform.anchoredPosition = new Vector3(
                    rectTransform.localPosition.x, 
                    -rectTransform.sizeDelta.y,
                    rectTransform.localPosition.z);
            }
            else
            {
                finalYPosition = -rectTransform.sizeDelta.y;
            }

            //return rectTransform.DOAnchorPosY(finalYPosition, _duration);
            return rectTransform.DOMove(default, _duration);
            
        }
    }
}
