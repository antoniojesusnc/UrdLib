using Urd.Services.EventBus;
using Urd.Tutorial;

namespace Urd.Events
{
    public class OnBeginTutorialEvent : IEventBusMessage
    {
        public ITutorialStep TutorialStep { get; private set; }

        public OnBeginTutorialEvent(ITutorialStep tutorialStep)
        {
            TutorialStep = tutorialStep;
        }
    }
    
    public class OnFinishTutorialEvent : IEventBusMessage
    {
        public ITutorialStep TutorialStep { get; private set; }

        public OnFinishTutorialEvent(ITutorialStep tutorialStep)
        {
            TutorialStep = tutorialStep;
        }
    }
    
    public class OnTutorialStepsRevaluationEvent : IEventBusMessage
    {
        public OnTutorialStepsRevaluationEvent()
        {
        }
    }
}
