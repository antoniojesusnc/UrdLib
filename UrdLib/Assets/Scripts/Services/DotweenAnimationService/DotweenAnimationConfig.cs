using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using Urd.Animation;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "DotweenAnimationConfig", menuName = "Urd/Services/Dotween Animation Config", order = 1)]
    public class DotweenAnimationConfig : ScriptableObject
    {
        [field: SerializeField, DisplayInspector]
        public List<TweenAnimation> AnimationList { get; private set; }

        public bool TryGetAnimation<T>(Enum enumerable, out T animation) where T : class, IBaseTweenAnimation
        {
            animation = AnimationList.Find(tween => tween.GetAnimationType().Equals(enumerable)) as T;
            return animation != null;
        }
    }
}