using System;
using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "UITweenAnimationBeingHit", menuName = "Urd/Animations/UI/UITweenAnimationBeingHitWhiteFlash", order = 1)]
    [Serializable]
    public class UITweenAnimationBeingHitWhiteFlash : UITweenAnimationBeingHit
    {
        public override Tween DoAnimation(RectTransform character, UIEffect effect)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(
                DOTween.To(
                    (value) => effect.colorAlpha = value,
                    0, 
                    1, 
                    Duration*0.5f)).SetEase(Ease).SetLoops(2, LoopType.Yoyo);

            return sequence;
        }
    }
}
