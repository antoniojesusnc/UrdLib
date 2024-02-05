using System;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    [Serializable]
    public abstract class TweenAnimation<TEnum> : TweenAnimation, IBaseTweenAnimation<TEnum> where TEnum : Enum, IConvertible
    {
        [field: Header("General Config"), SerializeField]
        public TEnum AnimationType { get; private set; }
        [SerializeField] protected float _duration;
        [SerializeField] protected Ease _ease = Ease.OutQuad;
        public override Enum GetAnimationType() => AnimationType;
    }
    
    public abstract class TweenAnimation : ScriptableObject, IBaseTweenAnimation
    {
        public abstract Enum GetAnimationType();
    }
}