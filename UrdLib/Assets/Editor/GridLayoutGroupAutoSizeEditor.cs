using Urd.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Urd.Editor
{
    [CustomEditor(typeof(GridLayoutAutoResize), true)]
    [CanEditMultipleObjects]
    public class GridLayoutGroupAutoSizeEditor : GridLayoutGroupEditor
    {
        SerializedProperty _autoResizeX;
        SerializedProperty _autoResizeY;

        protected override void OnEnable()
        {
            base.OnEnable();
            _autoResizeX = serializedObject.FindProperty("_autoResizeX");
            _autoResizeY = serializedObject.FindProperty("_autoResizeY");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_autoResizeX, true);
            EditorGUILayout.PropertyField(_autoResizeY, true);
            
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();

            if (GUILayout.Button("Resize"))
            {
                (target as GridLayoutAutoResize)?.Resize();
            }
        }
        
    }
}
