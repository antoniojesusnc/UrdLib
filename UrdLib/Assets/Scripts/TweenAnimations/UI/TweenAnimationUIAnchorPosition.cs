using System;
using DG.Tweening;
using Urd.Animation;
using UnityEngine;

namespace Urd
{
    public abstract class TweenAnimationUIAnchorPosition : TweenAnimation, ITweenAnimation<RectTransform, Vector2>
    {
        public abstract Tween DoAnimation(RectTransform uiElement, Vector2 finalPosition);
    }
}
