using System;
using Urd.Services;
using UnityEngine;

namespace Urd.Tutorial
{
    [Serializable]
    public class TutorialStepDependency : TutorialDependency, 
        IEventBusObservable<OnFinishTutorialEvent>
    {
        [field: SerializeField] public GamePlayTutorialStepConfig StepConfig { get; private set; }
        protected override bool IsMetInternal() => StepConfig != null && StepConfig.TutorialStep.HasPlayerCompleted;
        
        public override void AddRevaluationListeners()
        {
            StaticServiceLocator.Get<IEventBusService>().Subscribe(observer: this);
        }

        public override void RemoveRevaluationListeners()
        {
            StaticServiceLocator.Get<IEventBusService>().Subscribe(observer: this);
        }

        public override string Message 
            => IsMet
                ? $"Tutorial step {StepConfig.TutorialStep.Id} is completed" 
                : $"Tutorial step {StepConfig.TutorialStep.Id} is not completed";

        public void OnNewEvent(OnFinishTutorialEvent newEvent)
        {
            if (newEvent?.TutorialStep != StepConfig?.TutorialStep)
                return;
            
            TutorialModule.RevaluateTutorialStep(TutorialStep);
        }
    }
}