using UnityEditor;
using UnityEngine;

namespace Urd.Editor
{
    public class ShowNotifyOrLog
    {
        public static void Message(string msg)
        {
            if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}
