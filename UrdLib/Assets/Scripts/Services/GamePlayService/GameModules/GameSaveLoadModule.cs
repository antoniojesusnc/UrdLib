using System;
using Urd.Services;

namespace Urd
{
    [Serializable]
    public abstract class GameSaveLoadModule : GamePlayServiceModule
    {
        public abstract void LoadOfflineProgress(Action onFinishLoadOfflineData);
    }
}