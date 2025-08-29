using System;
using Urd.Services;

namespace Urd
{
    [Serializable]
    public abstract class GameSaveLoadModule : GamePlayModule
    {
        public abstract IPlayerModel LoadOfflineProgress();

        public abstract void SaveData();
    }
}