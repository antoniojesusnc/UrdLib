using DG.Tweening;
using UnityEngine;
using Urd.Animation;

namespace Urd.Animations
{
    public abstract class TweenAnimationGameObject : TweenAnimation,  ITweenAnimation<GameObject>
    {
        public abstract Tween DoAnimation(GameObject rectTransform);
    }
}