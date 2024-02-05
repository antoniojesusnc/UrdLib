using System;
using DG.Tweening;
using Urd.Services;

namespace Urd.Animation
{
    public interface IBaseTweenAnimation
    {
        Enum GetAnimationType();
    }
    
    public interface IBaseTweenAnimation<TEnum> : IBaseTweenAnimation where TEnum : Enum, IConvertible
    {
        TEnum AnimationType { get; }
    }
    
    public interface ITweenAnimation : IBaseTweenAnimation
    {
        public Tween DoAnimation();
    }
    
    public interface ITweenAnimation<T1> : IBaseTweenAnimation
    {
        public Tween DoAnimation(T1 rectTransform);
    }
    
    public interface ITweenAnimation<T1, T2> : IBaseTweenAnimation
    {
        public Tween DoAnimation(T1 parameter1, T2 parameter2);
    }
    
    public interface ITweenAnimation<T1, T2, T3> : IBaseTweenAnimation
    {
        public Tween DoAnimation(T1 parameter1, T2 parameter2, T3 parameter3);
    }
}