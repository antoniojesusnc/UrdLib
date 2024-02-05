using UnityEditor;

namespace USEFUL
{
    public class FSMWindows : EditorWindow
    {
        [MenuItem("Norna Games/Useful/FSM/FSM Manager")]
        public static void ShowWindows()
        {
            EditorWindow.GetWindow(typeof(FSMWindows));
        }
        
        void OnGUI () {
            
        }
    }
}