using System;
using Urd.Gameplay;
using Urd.Services;

namespace Urd
{
    [Serializable]
    public abstract class GameSaveLoadModule<T> : GameSaveLoadModule where T : class, IPlayerModel
    {
        protected string PLAYER_KEY = "PLAYER_KEY";

        public override IPlayerModel LoadOfflineProgress()
        {
            var saveLoadService = StaticServiceLocator.Get<ISaveLoadService>();
            if (!saveLoadService.HasKey(PLAYER_KEY))
            {
                return CreateNewPlayer();
            }

            if(saveLoadService.TryLoad<T>(PLAYER_KEY, out T newPlayerModel))
            {
                return newPlayerModel;
            }

            return CreateNewPlayer();
        }
        
        protected abstract IPlayerModel CreateNewPlayer();

        public override void SaveData()
        {
            var playerModel = StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<T>();
            StaticServiceLocator.Get<ISaveLoadService>().Save(PLAYER_KEY, playerModel);
        }
    }
}