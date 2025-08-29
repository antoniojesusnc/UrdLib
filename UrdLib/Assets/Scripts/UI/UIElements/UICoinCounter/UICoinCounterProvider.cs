using System;

namespace Urd
{
    public abstract class UICoinCounterProvider : IDisposable
    {
        protected UICoinCounter _uiCoinCounter;
        public abstract string GetCoinsString();
      
        public virtual void Init(UICoinCounter uiCoinCounter)
        {
            _uiCoinCounter = uiCoinCounter;
        }

        public virtual void Dispose()
        {
        }
    }
}
