using System;
using DG.Tweening;
using Urd.UI;
using UnityEngine;

namespace Urd.Animation
{
    public abstract class TweenAnimationHPBarDecrease : TweenAnimation, ITweenAnimation<UIHPBar>
    {
        public abstract Tween DoAnimation(UIHPBar uiBar);
    }
}
