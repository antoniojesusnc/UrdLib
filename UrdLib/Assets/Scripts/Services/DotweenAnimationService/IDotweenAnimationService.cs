using System;
using Urd.Animation;

namespace Urd.Services
{
    public interface IDotweenAnimationService : IBaseService
    {
        public bool TryGetAnimation<T>(Enum enumerable, out T animation) where T : class, IBaseTweenAnimation;
    }
}
