using System;
using System.Collections.Generic;
using Urd.Gameplay;
using Urd.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Urd.Services
{
    [Serializable]
    public class GamePlayService : BaseService, IGamePlayService
    {
        public override int LoadPriority => 200;

        public bool IsLoading { get; private set; }
        public event Action OnFinishLoad;
        
        public PlayerModel PlayerModel { get; private set; }

        [SerializeReference, SubclassSelector]
        private List<IGamePlayServiceModule> _gamePlayServiceModule;

        public override void Init()
        {
            InitNewPlayer();

            base.Init();
            InitModules();

            LoadData();
        }

        protected virtual void InitNewPlayer()
        {
        }

        private void InitModules()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i].Init();
            }
        }
        
        public override void Dispose()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.Dispose();
            }
            base.Dispose();
        }

        public virtual void LoadData()
        {
            IsLoading = true;
            
            GetModule<GameSaveLoadModule>().LoadOfflineProgress(OnFinishLoadOfflineData);
        }

        public T GetModule<T>() where T : class, IGamePlayServiceModule 
        {
            return _gamePlayServiceModule.Find(module => module.GetType().IsAssignableFrom(typeof(T))) as T;
        }

        private void OnFinishLoadOfflineData()
        {
            PlayerModel.Init();

            IsLoading = false;
            OnFinishLoad?.Invoke();

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
            }
            else
            {
                BeginGame();
            }
        }

        private void OnActiveSceneChanged(Scene previeusScene, Scene newScene)
        {
            if (newScene.buildIndex == 1)
            {
                BeginGame();
            }
        }

        private void BeginGame()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.BeginGame();
            }
        }
    }
}
