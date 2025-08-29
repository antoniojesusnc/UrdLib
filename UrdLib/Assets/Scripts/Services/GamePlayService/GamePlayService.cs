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

        private IPlayerModel _playerModel;

        [SerializeReference, SubclassSelector]
        private List<IGamePlayModule> _gamePlayServiceModule;

        public override void Init()
        {
            base.Init();
            InitModules();

            LoadData();
        }
        
        public T GetPlayerModel<T>() where T : class, IPlayerModel
        {
            return _playerModel as T;
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
            
            _playerModel = GetModule<GameSaveLoadModule>().LoadOfflineProgress();
            OnFinishLoadOfflineData();
        }

        public T GetModule<T>() where T : class, IGamePlayModule 
        {
            return _gamePlayServiceModule.Find(module => typeof(T).IsAssignableFrom(module.GetType())) as T;
        }

        private void OnFinishLoadOfflineData()
        {
            _playerModel.Init();

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
                _gamePlayServiceModule[i]?.BeginGameCoroutine();
            }
        }
    }
}
