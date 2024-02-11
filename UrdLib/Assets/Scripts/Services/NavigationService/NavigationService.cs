using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Urd.Error;
using Urd.Navigation;

namespace Urd.Services
{
    [Serializable]
    public class NavigationService : BaseService, INavigationService
    {     
        public override int LoadPriority => 80;

        [field: SerializeReference, SubclassSelector]
        public List<INavigationManager> NavigationManagers { get; private set; } = new();

        public event Action<INavigableModel> OnNavigableOpened;
        public event Action OnCloseAll;

        public NavigationService()
        {
            NavigationManagers.Add(new NavigationSceneManager());
            NavigationManagers.Add(new NavigationPopupManager());
            NavigationManagers.Add(new NavigationBoomerangManager());
        }
        
        public override void Init()
        {
            base.Init();
            
            for (int i = 0; i < NavigationManagers.Count; i++)
            {
                NavigationManagers[i].Init();
            }
        }

        public void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigableCallback)
        {
            if (!TryGetManager(navigableModel.GetType(), out var navigationManager))
            {
                var errorMessage = $"[NavigationService] Cannot find manager for model {navigableModel}";
                var error = new ErrorModel(errorMessage, ErrorCode.Error_404_Not_Found);
                onOpenNavigableCallback?.Invoke(error);
                return;
            }

            navigationManager.Open(navigableModel, (errorModel) => OnOpenNavigable(navigableModel as SceneModel, errorModel, onOpenNavigableCallback));
        }

        private void OnOpenNavigable(SceneModel sceneModel, ErrorModel errorModel,
            Action<ErrorModel> onOpenNavigable)
        {
            if (errorModel.IsSuccess)
            {
                OnNavigableOpened?.Invoke(sceneModel);
            }
            onOpenNavigable?.Invoke(errorModel);
        }

        public bool IsOpen(INavigableModel navigableModel)
        {
            if (!TryGetManager(navigableModel.GetType(), out var navigationManager))
            {
                var errorMessage = $"[NavigationService] Cannot find manager for model {navigableModel}";
                var error = new ErrorModel(errorMessage, ErrorCode.Error_404_Not_Found);
                return false;
            }

            return navigationManager.IsOpen(navigableModel);
        }

        public void CloseAll<T>() where T : INavigableModel
        {
            if (!TryGetManager(typeof(T), out var navigationManager))
            {
                var errorMessage = $"[NavigationService] Cannot find manager for model {typeof(T)}";
                var error = new ErrorModel(errorMessage, ErrorCode.Error_404_Not_Found);
                return;
            }

            navigationManager.CloseAll();
            OnCloseAll?.Invoke();
        }

        public void Close(INavigableModel navigableModel, Action<ErrorModel> callback = null)
        {
            if (!TryGetManager(navigableModel.GetType(), out var navigationManager))
            {
                var errorMessage = $"[NavigationService] Cannot find manager for model {navigableModel}";
                var error = new ErrorModel(errorMessage, ErrorCode.Error_404_Not_Found);
                callback?.Invoke(error);
                return;
            }

            navigationManager.Close(navigableModel, callback);
        }
        
        private bool TryGetManager(Type navigableType, out INavigationManager navigationManager)
        {
            navigationManager =
                NavigationManagers.Find(manager => manager.ModelType.IsAssignableFrom(navigableType));
                //NavigationManagers.Find(manager => manager.ModelType.IsInstanceOfType(navigableModelModel));
            return navigationManager != null;
        }
    }
}