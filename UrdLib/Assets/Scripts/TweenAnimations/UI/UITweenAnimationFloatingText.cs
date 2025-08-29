using DG.Tweening;
using Urd.Animation;
using TMPro;
using UnityEngine;

namespace Urd.Animation
{
    public abstract class UITweenAnimationFloatingText : TweenAnimation, ITweenAnimation<RectTransform, TextMeshProUGUI>
    {
        public abstract Tween DoAnimation(RectTransform uiElement, TextMeshProUGUI text);
    }
}
