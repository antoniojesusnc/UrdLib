using System;

namespace Urd
{
    [Serializable]
    public abstract class TutorialExtraBehavior : ITutorialExtraBehavior
    {
        public virtual void Init()
        {
            
        }

        public virtual void Begin()
        {
        }

        public virtual void Finish()
        {
        }

        public void Dispose()
        {

        }
    }
}