using System;
using Unity.Services.Core;

namespace Urd.Services
{
    [Serializable]
    public class UnityService : BaseService, IUnityService
    {
        public override int LoadPriority => 50;

        public event Action<bool> OnGamePaused;
        public event Action<bool> OnGameFocus;

        public override void Init()
        {
            base.Init();
            
            UnityServices.InitializeAsync();
        }

        public void CallOnGameFocus(bool focus)
        {
            OnGameFocus?.Invoke(focus);
        }

        public void CallOnGamePaused(bool paused)
        {
            OnGamePaused?.Invoke(paused);
        }
    }
}
