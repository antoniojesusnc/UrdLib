using MyBox;
using Urd.Services;
using UnityEngine;
using Urd.Gameplay;

namespace Urd.Tutorial
{
    public class TutorialStep : ITutorialStep
    {   
        [field: SerializeField, ReadOnly]
        public string Id { get; set; }

        [field: SerializeField]
        public bool IsActive { get; private set; }
        
        [field: SerializeField]
        public int ExecutionOrder { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public TutorialDependency[] Dependencies { get; private set; }
        
        [field: SerializeField]
        public GamePlayTutorialStepConfig[] CompleteTogether { get; private set; }

        public bool HasPlayerCompleted => TutorialModule.IsTutorialDone(this);

        protected GamePlayTutorialModule TutorialModule 
            => StaticServiceLocator.Get<IGamePlayService>().GetModule<GamePlayTutorialModule>();

        public virtual void Init()
        {
            
        }

        public virtual void Dispose()
        {
        }

        public virtual void Begin()
        {
        }

        public virtual void Finish()
        {
        }
    }
}