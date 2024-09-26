using UnityEngine;

namespace Urd.Utils
{
    public class ScreenCaptureUtilsView : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TakeScreenShot();
            }
        }

        private void TakeScreenShot()
        {
            ScreenCaptureUtils.TakeScreen();
        }
    }
}