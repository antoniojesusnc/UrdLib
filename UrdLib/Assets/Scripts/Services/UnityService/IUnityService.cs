using System;

namespace Urd.Services
{
    public interface IUnityService : IBaseService
    {
        event Action<bool> OnGamePaused;
        event Action<bool> OnGameFocus;

        void CallOnGamePaused(bool paused);
        void CallOnGameFocus(bool focus);
    }
}
