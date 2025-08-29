using System;
using DG.Tweening;
using Urd.UI;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationHPBarDecreaseVibration", menuName = "Urd/Animations/UI/TweenAnimationHPBarDecreaseVibration", order = 1)]
    [Serializable]
    public class TweenAnimationHPBarDecreaseVibration : TweenAnimationHPBarDecrease
    {
        [SerializeField]
        private Vector3 _bumpScale;
        
        public override Tween DoAnimation(UIHPBar uiBar)
        {
            var sequence = DOTween.Sequence();
            var rectTransform = uiBar.GetComponent<RectTransform>();
            sequence.AppendInterval(Delay);
            sequence.Append(rectTransform.DOScale( _bumpScale, Duration*0.5f));
            sequence.Append(rectTransform.DOScale( Vector3.one, Duration*0.5f));
            return sequence;
        }
    }
}
