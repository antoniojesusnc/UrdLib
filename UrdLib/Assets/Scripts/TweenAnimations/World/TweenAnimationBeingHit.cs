using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    public abstract class TweenAnimationBeingHit : TweenAnimation, ITweenAnimation<List<SpriteRenderer>>
    {
        public abstract Tween DoAnimation(List<SpriteRenderer> rectTransform);
    }
}
