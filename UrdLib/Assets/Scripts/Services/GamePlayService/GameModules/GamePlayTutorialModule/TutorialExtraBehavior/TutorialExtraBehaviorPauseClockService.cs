using System;
using Urd.Services;

namespace Urd
{
    [Serializable]
    public class TutorialExtraBehaviorPauseClockService : TutorialExtraBehavior
    {
        public override void Begin()
        {
            base.Begin();
            StaticServiceLocator.Get<IClockService>().SetPause(true);
        }

        public override void Finish()
        {
            base.Finish();
            StaticServiceLocator.Get<IClockService>().SetPause(false);
        }
    }
}