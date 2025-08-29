using System;
using Urd.UI;
using UnityEditor;
using UnityEditor.UI;

namespace Urd.Editor
{
    [CustomEditor(typeof(HoldableButton), true)]
    public class EditorHoldableButtons : ButtonEditor
    {
        private SerializedProperty _holdTime;
        private SerializedProperty _holdMaxSpeedTime;
        private SerializedProperty _timeToReachMaxSpeed;

        protected override void OnEnable()
        {
            base.OnEnable();
            _holdTime = serializedObject.FindProperty("_holdTime");
            _holdMaxSpeedTime = serializedObject.FindProperty("_holdMaxSpeedTime");
            _timeToReachMaxSpeed = serializedObject.FindProperty("_timeToReachMaxSpeed");
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_holdTime, true);
            EditorGUILayout.PropertyField(_holdMaxSpeedTime, true);
            EditorGUILayout.PropertyField(_timeToReachMaxSpeed, true);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}