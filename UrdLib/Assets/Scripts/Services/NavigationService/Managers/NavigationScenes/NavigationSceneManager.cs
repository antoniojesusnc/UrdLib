using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Urd.Error;
using Urd.Services;

namespace Urd.Navigation
{
    [Serializable]
    public class NavigationSceneManager : NavigationManager<SceneModel>
    {
        public override void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigable)
        {
            var sceneModel = navigableModel as SceneModel;
            
            if (!TryGetBuildSceneBuildIndex(sceneModel.Type.ToString(), out int buildIndex))
            {
                var error = new ErrorModel(
                    $"[NavigationPopupManager] Error when try to get the scene, scene type {sceneModel.Type}",
                    ErrorCode.Error_404_Not_Found, UnityWebRequest.Result.DataProcessingError);
                Debug.LogWarning(error.ToString());

                onOpenNavigable?.Invoke(error);
                return;
            }

            
            sceneModel.SetBuildIndex(buildIndex);
            StaticServiceLocator.Get<IAssetService>().LoadScene(sceneModel, 
                                                                sceneModel => OnLoadSceneCallback(sceneModel, onOpenNavigable));
        }

        private void OnLoadSceneCallback(SceneModel sceneModel, Action<ErrorModel> onOpenNavigable)
        {
            if (sceneModel.HasScene)
            {
                onOpenNavigable?.Invoke(new ErrorModel());
            }
        }

        public override void Close(INavigableModel navigableModel, Action<ErrorModel> onCloseNavigable)
        {
            var sceneModel = navigableModel as SceneModel;
            
            sceneModel.SetScene(SceneManager.GetSceneByName(sceneModel.Type.ToString()));
            
            StaticServiceLocator.Get<IAssetService>().UnLoadScene(sceneModel, 
                                                                  success => OnUnLoadSceneCallback(success, onCloseNavigable));
        }

        public override bool IsOpen(INavigableModel navigableModel)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == navigableModel.Type.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        private void OnUnLoadSceneCallback(bool success, Action<ErrorModel> onCloseNavigable)
        {
            onCloseNavigable?.Invoke(new ErrorModel());
        }

        private bool TryGetBuildSceneBuildIndex(string sceneModelId, out int buildIndex)
        {
            buildIndex = 0;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var sceneTemp = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneName = sceneTemp.Substring(sceneTemp.LastIndexOf("/")+1, sceneModelId.Length);
                if (sceneName == sceneModelId)
                {
                    buildIndex = i;
                    return true;
                }
            }
            return false;
        }
    }
}