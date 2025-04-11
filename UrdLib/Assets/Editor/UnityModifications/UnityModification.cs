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
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUIOnLeft);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIOnRight);
        }

        static void OnToolbarGUIOnLeft()
        {
            GUILayout.FlexibleSpace();

            if(GUILayout.Button(new GUIContent("Clean Boot", "Start Scene 1"), 
                                !EditorApplication.isPlaying
                                    ? ToolbarStyles.NormalButtonStyle
                                    : ToolbarStyles.ActivatedButtonStyle))
            {
                PlayGameFromInitialScene();
            }
        }

        static void OnToolbarGUIOnRight()
        {
            var index = -1;
            if (EditorSceneManager.sceneCountInBuildSettings > ++index)
                AddSceneButton(index);
            if (EditorSceneManager.sceneCountInBuildSettings > ++index)
                AddSceneButton(index);   
            if (EditorSceneManager.sceneCountInBuildSettings > ++index)
                AddSceneButton(index);   
            if (EditorSceneManager.sceneCountInBuildSettings > ++index)
                AddSceneButton(index);   
            if (EditorSceneManager.sceneCountInBuildSettings > ++index)
                AddSceneButton(index);   

            GUILayout.FlexibleSpace();
        }

        private static void AddSceneButton(int index)
        { ;
            if (GUILayout.Button(new GUIContent($"{index}", $"Open Scene {index}"),
                    !EditorApplication.isPlaying
                        ? ToolbarStyles.NormalButtonStyle
                        : ToolbarStyles.ActivatedButtonStyle))
            {
                OpenScene(index);
            }
        }

        private static void OpenScene(int index)
        {
            if (EditorApplication.isPlaying)
            {
                ShowNotifyOrLog.Message("Cannot be used in play mode");
                return;
            }
            
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(index);
                EditorSceneManager.OpenScene(scenePath);
            }
        }


        private static void PlayGameFromInitialScene()
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

            foreach (GameObject go in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                go.SetActive(false);

            SceneManager.LoadScene(0);
        }
    }
}