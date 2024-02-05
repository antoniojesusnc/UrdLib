using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using Urd.Animation;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "DotweenAnimationConfig", menuName = "Urd/Services/Dotween Animation Config", order = 1)]
    public class DotweenAnimationConfig : ScriptableObject
    {
        [field: SerializeField, DisplayInspector] public List<TweenAnimation> _animationList;

        public bool TryGetAnimation<T>(Enum enumerable, out T animation) where T : class, IBaseTweenAnimation
        {
            animation = _animationList.Find(tween => tween.GetAnimationType().Equals(enumerable)) as T;
            return animation != null;
        }
    }
}