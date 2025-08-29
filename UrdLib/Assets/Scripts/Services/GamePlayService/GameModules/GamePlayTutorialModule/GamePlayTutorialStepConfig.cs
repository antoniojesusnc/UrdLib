using UnityEditor;
using UnityEngine;

namespace Urd.Tutorial
{
    [CreateAssetMenu(menuName = "Urd/Gameplay/GamePlay TutorialStep Config", fileName = "GamePlayTutorialStepConfig", order = 1)]
    public class GamePlayTutorialStepConfig : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public ITutorialStep TutorialStep { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            TutorialStep.Id = path.Split('/')[path.Split('/').Length - 2].Split('.')[0] + "/"+name;
        }  
#endif
    }
}