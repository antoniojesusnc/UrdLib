using DG.Tweening;
using UnityEngine;

namespace Urd
{
    [CreateAssetMenu(fileName = "TweenAnimationUIMoveRectLine", menuName = "Urd/Animations/UI/TweenAnimationUIMoveRectLine", order = 1)]
    public class TweenAnimationUIMoveRectLine : TweenAnimationUIAnchorPosition
    {
        public override Tween DoAnimation(RectTransform uiElement, Vector2 finalPosition)
        {
            var sequence = DOTween.Sequence();
            uiElement.DOAnchorPos(finalPosition, Duration).SetEase(Ease).SetDelay(Delay);
            return sequence;
        }
    }
}
