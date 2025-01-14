using System;
using DG.Tweening;
using DG.Tweening.Core.Enums;
using UnityEngine;
using Urd.Animation;

namespace Urd.Services
{
    [Serializable]
    public class DotweenAnimationService : BaseService, IDotweenAnimationService
    {
        public override int LoadPriority => 90;

        [SerializeField] private DotweenAnimationConfig _dotweenAnimationConfig;

        public override void Init()
        {
            base.Init();
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
            DOTween.nestedTweenFailureBehaviour = NestedTweenFailureBehaviour.KillWholeSequence;
            DOTween.defaultRecyclable = true;
        }

        public void SetConfig(DotweenAnimationConfig dotweenAnimationConfig)
        {
            _dotweenAnimationConfig = dotweenAnimationConfig;
        }
        
        public bool TryGetAnimation<T>(Enum enumerable, out T animation) where T : class, IBaseTweenAnimation
        {
            return _dotweenAnimationConfig.TryGetAnimation(enumerable, out animation);
        }

    }
}
