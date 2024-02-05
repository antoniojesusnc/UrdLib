using System;
using UnityEngine;
using Urd.Animation;

namespace Urd.Services
{
    [Serializable]
    public class DotweenAnimationService : BaseService, IDotweenAnimationService
    {
        public override int LoadPriority => 90;

        [SerializeField] private DotweenAnimationConfig _dotweenAnimationConfig;
        
        public bool TryGetAnimation<T>(Enum enumerable, out T animation) where T : class, IBaseTweenAnimation
        {
            return _dotweenAnimationConfig.TryGetAnimation(enumerable, out animation);
        }
    }
}
