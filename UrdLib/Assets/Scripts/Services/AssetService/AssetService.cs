using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Urd.Navigation;

namespace Urd.Services
{
    [System.Serializable]
    public class AssetService : BaseService, IAssetService
    {
        public override int LoadPriority => 10;

        
            
        private IResourceLocator _resourceLocator;
        
        public override void Init()
        {
            IsLoaded = false;
            
            base.Init();

            Addressables.InitializeAsync().Completed += OnAddressableInitialize;
        }

        private void OnAddressableInitialize(AsyncOperationHandle<IResourceLocator> resourceLocator)
        {
            _resourceLocator = resourceLocator.Result;
            SetAsLoaded();
            
            UnityEngine.Debug.Log($"[AssetService] OnAddressableInitialize {resourceLocator.Status}");
        }

        public void LoadAsset<T>(string addressName, Action<T> assetCallback)
        {
            _resourceLocator.Locate(addressName, typeof(T), out var location);

            if (location != null && location.Count > 0)
            {
                if (location.Count > 1)
                {
                    Debug.LogWarning($"[AssetService] LoadAsset {addressName}, more than 1 asset with this name");
                }
                LoadAssetIntenal<T>(location[0], assetCallback);
                return;
            }

            LoadAssetIntenal<T>(addressName, assetCallback);
        }
        
        private void LoadAssetIntenal<T>(IResourceLocation resourceLocation, Action<T> assetCallback)
        {
            Addressables.LoadAssetAsync<T>(resourceLocation).Completed += 
                task => OnLoadAsset<T>(task, resourceLocation.PrimaryKey, assetCallback);
        }

        private void LoadAssetIntenal<T>(string addressName, Action<T> assetCallback)
        {
            Addressables.LoadAssetAsync<T>(addressName).Completed += (task) => OnLoadAsset<T>(task, addressName, assetCallback);
        }

        private void OnLoadAsset<T>(AsyncOperationHandle<T> task, string addressableName, Action<T> assetCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadAsset {addressableName} cannot Instantiate");
                assetCallback?.Invoke(default(T));
                return;
            }
            assetCallback?.Invoke(task.Result);
        }

        public void LoadAssetByLabel<T>(string labelName, Action<List<T>> assetsCallback)
        {
            _resourceLocator.Locate(labelName, typeof(T), out var location);
            
            if (location != null && location.Count > 0)
            {
                LoadAssetByLabelInternal<T>(labelName, location, assetsCallback);
                return;
            }

            LoadAssetByLabelInternal<T>(labelName, assetsCallback);
        }

        private void LoadAssetByLabelInternal<T>(string labelName, IList<IResourceLocation> locations, Action<List<T>> assetsCallback)
        {
            Addressables.LoadAssetsAsync<T>(locations, null).Completed +=
                objects => OnLoadAssetByLabelInternal<T>(objects, labelName, assetsCallback);
        }

        private void LoadAssetByLabelInternal<T>(string labelName, Action<List<T>> assetsCallback)
        {
            Addressables.LoadAssetsAsync<T>(labelName, null).Completed +=
                objects => OnLoadAssetByLabelInternal(objects, labelName, assetsCallback);
        }

        private void OnLoadAssetByLabelInternal<T>(AsyncOperationHandle<IList<T>> objects, string labelName, Action<List<T>> assetsCallback)
        {
            if (objects.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadAsset {labelName} cannot Instantiate");
                assetsCallback?.Invoke(new List<T>());
                return;
            }
            assetsCallback?.Invoke(new List<T>(objects.Result));
        }

        public void LoadScene(SceneModel sceneModel, Action<SceneModel> onLoadSceneCallback)
        {
            if (sceneModel.IsInBuildIndex)
            {
                LoadSceneFromBuildIndex(sceneModel, onLoadSceneCallback);
            }
            else
            {
                LoadSceneFromAddressable(sceneModel, onLoadSceneCallback);
            }
        }

        private void LoadSceneFromBuildIndex(SceneModel sceneModel, Action<SceneModel> onLoadSceneCallback)
        {
            SceneManager.LoadSceneAsync(sceneModel.BuildIndex, LoadSceneMode.Additive).completed += 
                task => OnLoadSceneFromBuildIndex(task, sceneModel, onLoadSceneCallback); 
        }

        private void OnLoadSceneFromBuildIndex(AsyncOperation task, SceneModel sceneModel,
            Action<SceneModel> onLoadSceneCallback)
        {
            if (!task.isDone)
            {
                Debug.LogWarning($"[AssetService] OnLoadSceneFromBuildIndex {sceneModel.Type} cannot Instantiate");
                onLoadSceneCallback?.Invoke(sceneModel);
                return;
            }

            var scene = SceneManager.GetSceneByBuildIndex(sceneModel.BuildIndex);
            sceneModel.SetScene(scene);
            SceneManager.SetActiveScene(scene);
            onLoadSceneCallback.Invoke(sceneModel);
        }

        private void LoadSceneFromAddressable(SceneModel sceneModel, Action<SceneModel> onLoadSceneCallback)
        {
            _resourceLocator.Locate(sceneModel, typeof(SceneInstance), out var location);

            if (location != null && location.Count > 0)
            {
                if (location.Count > 1)
                {
                    Debug.LogWarning($"[AssetService] LoadSceneFromAddressable {sceneModel}, more than 1 asset with this name");
                }
                LoadSceneInternal(sceneModel, location[0], onLoadSceneCallback);
                return;
            }

            LoadSceneInternal(sceneModel, onLoadSceneCallback);
        }
        
        private void LoadSceneInternal(SceneModel sceneModel, IResourceLocation resourceLocation,
            Action<SceneModel> onLoadSceneCallback)
        {
            Addressables.LoadSceneAsync(resourceLocation, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += 
                task => OnLoadSceneFromAddressable(task, sceneModel, onLoadSceneCallback);
        }

        private void LoadSceneInternal(SceneModel sceneModel, Action<SceneModel> onLoadSceneCallback)
        {
            Addressables.LoadSceneAsync(sceneModel.Type, UnityEngine.SceneManagement.LoadSceneMode.Additive).Completed += 
                task => OnLoadSceneFromAddressable(task, sceneModel, onLoadSceneCallback);
        }

        private void OnLoadSceneFromAddressable(AsyncOperationHandle<SceneInstance> task, SceneModel sceneModel, Action<SceneModel> onLoadSceneCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadSceneFromAddressable {sceneModel.Type} cannot Instantiate");
                onLoadSceneCallback?.Invoke(sceneModel);
                return;
            }
            sceneModel.SetSceneInstance(task.Result);
            onLoadSceneCallback?.Invoke(sceneModel);
        }

        public void UnLoadScene(SceneModel sceneModel, Action<bool> onUnloadSceneCallback)
        {
            if (!sceneModel.HasScene)
            {
                Debug.LogWarning($"[AssetService] UnLoadScene {sceneModel.Type} doesn't have a scene");
                onUnloadSceneCallback.Invoke(false);
                return;
            }

            if (sceneModel.Scene.IsValid())
            {
                UnLoadSceneFromBuildIndex(sceneModel, onUnloadSceneCallback);
            }
            else
            {
                UnloadSceneFromAddressable(sceneModel, onUnloadSceneCallback);
            }
        }

        private void UnLoadSceneFromBuildIndex(SceneModel sceneModel, Action<bool> onUnloadSceneCallback)
        {
            SceneManager.UnloadSceneAsync(sceneModel.Scene).completed += 
                task => OnUnLoadSceneFromBuildIndex(task, sceneModel, onUnloadSceneCallback);
        }

        private void OnUnLoadSceneFromBuildIndex(AsyncOperation task, SceneModel sceneModel, Action<bool> onUnloadSceneCallback)
        {
            if (!task.isDone)
            {
                Debug.LogWarning($"[AssetService] OnUnLoadSceneFromBuildIndex {sceneModel.Type} cannot be unloaded");
                onUnloadSceneCallback?.Invoke(false);
                return;
            }

            sceneModel.CleanScene();
            onUnloadSceneCallback.Invoke(true);
        }


        private void UnloadSceneFromAddressable(SceneModel sceneModel, Action<bool> onLoadSceneCallback)
        {
            Addressables.UnloadSceneAsync(sceneModel.SceneInstance).Completed += 
                task => OnUnloadSceneFromAddressable(task, sceneModel, onLoadSceneCallback);
        }

        private void OnUnloadSceneFromAddressable(AsyncOperationHandle<SceneInstance> task, SceneModel sceneModel, Action<bool> onLoadSceneCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadScene {sceneModel.SceneInstance.Scene.name} cannot unload");
                onLoadSceneCallback?.Invoke(false);
                return;
            }
            sceneModel.CleanScene();
            onLoadSceneCallback?.Invoke(true);
        }

        public void Instantiate(string addressName, Transform parent, Action<GameObject> instantiateCallback)
        {
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(addressName, parent, instantiateCallback);
                return;
            }
            
            _resourceLocator.Locate(addressName, typeof(GameObject), out var location);

            if(location != null && location.Count > 0)
            {
                if(location.Count > 1)
                {
                    Debug.LogWarning($"[AssetService] Instantiate {addressName}, more than 1 asset with this name");
                }
                InstantiateInternal(location[0], parent, instantiateCallback);
                return;
            }

            InstantiateInternal(addressName, parent, instantiateCallback);
        }

        public void Instantiate(GameObject prefab, Transform parent, Action<GameObject> instantiateCallback)
        {
            
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(prefab, parent, instantiateCallback);
                return;
            }
            
            var newGameObject = GameObject.Instantiate(prefab, parent);
            instantiateCallback.Invoke(newGameObject);
        }
        
        public void Instantiate<T>(T prefab, Transform parent, Action<T> instantiateCallback) where T : Behaviour
        {
            
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(prefab, parent, instantiateCallback);
                return;
            }
            
            var newGameObject = GameObject.Instantiate<T>(prefab, parent);
            instantiateCallback.Invoke(newGameObject);
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        private void InstantiateInternal(string addressName, Transform parent, Action<GameObject> instantiateCallback)
        {
            Addressables.InstantiateAsync(addressName, parent).Completed += (task) 
                => OnInstantiate(task, addressName, instantiateCallback);
        }

        private void InstantiateInternal(IResourceLocation resourceLocation, Transform parent, Action<GameObject> instantiateCallback)
        {
            Addressables.InstantiateAsync(resourceLocation, parent).Completed += (task) 
                => OnInstantiate(task, resourceLocation.InternalId, instantiateCallback);
        }

        private void OnInstantiate(AsyncOperationHandle<GameObject> task, string addressableName, Action<GameObject> instantiateCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnInstantiate {addressableName} cannot Instantiate");
                instantiateCallback?.Invoke(null);
                return;
            }
            instantiateCallback?.Invoke(task.Result);
        }
    }
}