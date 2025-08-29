using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Urd.Animation
{
    public abstract class TweenAnimation : ScriptableObject, IBaseTweenAnimation
    {
        [field: SerializeField] 
        public virtual float Duration { get; protected set; }  
        [field: SerializeField] 
        public virtual float Delay { get; protected  set; }

        [field:SerializeField] 
        protected Ease Ease { get; private set; }
    }
}