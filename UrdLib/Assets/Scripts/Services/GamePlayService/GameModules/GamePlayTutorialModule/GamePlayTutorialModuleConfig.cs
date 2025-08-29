using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urd.Tutorial
{
    [CreateAssetMenu(menuName = "Urd/Gameplay/GamePlayTutorialModuleConfig", fileName = "GamePlayTutorialModuleConfig", order = 1)]
    public class GamePlayTutorialModuleConfig : ScriptableObject
    {
        [field: SerializeField] public Canvas CanvasPrefab { get; private set; }
        [field: SerializeField] public bool EnableTutorial { get; private set; } = true;

        [field: SerializeField, ReadOnly]
        public List<GamePlayTutorialStepConfig> TutorialSteps { get; private set; } = new();
        
        [field: SerializeField]
        public List<GamePlayTutorialStepConfig> ReadyToBattleSteps { get; private set; } = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string parentFolder = path[..path.LastIndexOf('/')];
            
            const string filter = " t:GamePlayTutorialStepConfig";
            string[] searchInFolders = { parentFolder };
            string[] guids = AssetDatabase.FindAssets(filter, searchInFolders);

            TutorialSteps = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(assetPath => assetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<GamePlayTutorialStepConfig>()
                .ToList();
        }
#endif
    }
}