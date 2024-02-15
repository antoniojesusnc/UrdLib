using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Urd.Navigation;

namespace Urd.Editor
{
    [CustomEditor(typeof(UIBoomerangConfig))]
    public class UIBoomerangConfigCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawAddAllBoomerangButton();
        }

        private void DrawAddAllBoomerangButton()
        {
            if (GUILayout.Button("Add All Boomerangs"))
            {
                var allUi = AssetsUtils.GetAllPrefabThatHas<UIBoomerangView>();
                var uiBoomerangConfig = target as UIBoomerangConfig;
                uiBoomerangConfig.Boomerangs.Clear();
                uiBoomerangConfig.Boomerangs.AddRange(allUi);

                AssetDatabase.SaveAssets();
            }
        }
    }
}
