using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace Urd.Editor.Utils
{
    [InitializeOnLoad]
    public static class UnityModification
    {
        static bool BootOpen
        {
            get { return EditorPrefs.HasKey("BootOpen") && EditorPrefs.GetBool("BootOpen"); }
            set { EditorPrefs.SetBool("BootOpen", value); }
        }
        
        static UnityModification()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if(GUILayout.Button(new GUIContent("Clean Boot", "Start Scene 1"), 
                                !EditorApplication.isPlaying
                                    ? ToolbarStyles.NormalButtonStyle
                                    : ToolbarStyles.ActivatedButtonStyle))
            {
                OpenInitialScene();
            }
        }
        
        
        private static void OpenInitialScene()
        {
            if (EditorApplication.isPlaying)
            {
                ShowNotifyOrLog.Message("Cannot be used in play mode");
                return;
            }
            
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                BootOpen = true;
                
                EditorApplication.isPlaying = true;
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadFirstSceneAtGameBegins()
        {
            if (!BootOpen)
                return;

            BootOpen = false;
            
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
                return;
            }

            foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
                go.SetActive(false);

            SceneManager.LoadScene(0);
        }
    }
}