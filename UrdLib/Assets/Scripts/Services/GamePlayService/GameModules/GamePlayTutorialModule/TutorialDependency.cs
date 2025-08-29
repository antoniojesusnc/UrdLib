using System;
using Urd.Services;
using UnityEngine;
using Urd.Gameplay;

namespace Urd.Tutorial
{
    [Serializable]
    public abstract class TutorialDependency
    {
        [field: SerializeField] public bool Negate { get; private set; }
        
        public bool IsMet => Negate ? !IsMetInternal() : IsMetInternal();
        
        protected GamePlayTutorialModule TutorialModule 
            => StaticServiceLocator.Get<IGamePlayService>().GetModule<GamePlayTutorialModule>();
        
        public ITutorialStep TutorialStep { get; set; }
        protected abstract bool IsMetInternal();
        public abstract void AddRevaluationListeners();
        public abstract void RemoveRevaluationListeners();

        public abstract string Message { get; }
    }
}