using Urd.Services.EventBus;
using Urd.Tutorial;

namespace Urd
{
    public class OnFinishTutorialEvent : IEventBusMessage
    {
        public ITutorialStep TutorialStep { get; private set; }
        public OnFinishTutorialEvent(ITutorialStep tutorialStep)
        {
            TutorialStep = tutorialStep;
        }
    }
}
