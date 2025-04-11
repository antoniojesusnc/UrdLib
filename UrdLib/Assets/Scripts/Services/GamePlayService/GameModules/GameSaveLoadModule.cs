using System;
using Urd.Services;

namespace Urd
{
    [Serializable]
    public abstract class GameSaveLoadModule : GamePlayServiceModule
    {
        public abstract IPlayerModel LoadOfflineProgress();

        public abstract void SaveData();
    }
}