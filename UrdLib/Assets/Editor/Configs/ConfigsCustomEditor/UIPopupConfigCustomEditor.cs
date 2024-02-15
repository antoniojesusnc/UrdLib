using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Urd.Navigation;

namespace Urd.Editor
{
    [CustomEditor(typeof(UIPopupConfig))]
    public class UIPopupConfigCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawAddAllPopupButton();
        }

        private void DrawAddAllPopupButton()
        {
            if (GUILayout.Button("Add All Popups"))
            {
                var allUi = AssetsUtils.GetAllPrefabThatHas<UIPopupView>();
                var popupConfig = target as UIPopupConfig;
                popupConfig.PopupViews.Clear();
                popupConfig.PopupViews.AddRange(allUi);

                AssetDatabase.SaveAssets();
            }
        }
    }
}
