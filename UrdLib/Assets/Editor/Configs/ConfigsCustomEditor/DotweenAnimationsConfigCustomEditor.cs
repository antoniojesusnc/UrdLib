using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Urd.Animation;
using Urd.Navigation;
using Urd.Services;

namespace Urd.Editor
{
    [CustomEditor(typeof(DotweenAnimationConfig))]
    public class DotweenAnimationsConfigCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawAddAllDotweeenAnimationsButton();
        }

        private void DrawAddAllDotweeenAnimationsButton()
        {
            if (GUILayout.Button("Add All Dotween Animations"))
            {
                var allUi = AssetsUtils.GetAllPrefabThatHas<TweenAnimation>();
                var dotweenAnimationConfig = target as DotweenAnimationConfig;
                dotweenAnimationConfig.AnimationList.Clear();
                dotweenAnimationConfig.AnimationList.AddRange(allUi);

                AssetDatabase.SaveAssets();
            }
        }
    }
}
