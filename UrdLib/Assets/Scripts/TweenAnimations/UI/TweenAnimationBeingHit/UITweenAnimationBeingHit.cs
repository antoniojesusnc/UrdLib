using System;
using Coffee.UIEffects;
using DG.Tweening;
using Urd.Animation;
using UnityEngine;

namespace Urd.Animation
{
    public abstract class UITweenAnimationBeingHit : TweenAnimation, ITweenAnimation<RectTransform, UIEffect>
    {
        public abstract Tween DoAnimation(RectTransform character, UIEffect effect);
    }
}
