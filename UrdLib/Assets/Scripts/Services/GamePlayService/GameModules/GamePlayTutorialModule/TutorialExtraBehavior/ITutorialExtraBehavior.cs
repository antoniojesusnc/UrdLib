using System;

namespace Urd
{
    public interface ITutorialExtraBehavior : IDisposable
    {
        void Init();
        void Begin();
        void Finish();
    }
}