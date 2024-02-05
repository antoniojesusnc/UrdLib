using System;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Urd.Navigation
{
    public class SceneModel : INavigableModel
    {
        private const int EMPTY_BUILD_INDEX = -1;

        public Enum Type { get; private set; }

        public bool IsInBuildIndex => BuildIndex >= 0;
        public int BuildIndex { get; protected set; } = EMPTY_BUILD_INDEX;

        public SceneInstance SceneInstance { get; protected set; }
        public UnityEngine.SceneManagement.Scene Scene { get; protected set; }
        public bool HasScene => SceneInstance.Scene.IsValid() || Scene.IsValid();
        public SceneModel(SceneTypes sceneTypes)
        {
            Type = sceneTypes;
        }

        public void SetScene(UnityEngine.SceneManagement.Scene scene)
        {
            Scene = scene;
        }
        
        public void SetSceneInstance(SceneInstance sceneInstance)
        {
            SceneInstance = sceneInstance;
        }
        
        public void SetBuildIndex(int buildIndex)
        {
            BuildIndex = buildIndex;
        }
        
        public void CleanScene()
        {
            BuildIndex = EMPTY_BUILD_INDEX;
            Scene = new UnityEngine.SceneManagement.Scene();
            SceneInstance = new SceneInstance();
        }
    }
}