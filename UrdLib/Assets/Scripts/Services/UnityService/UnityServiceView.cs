using UnityEngine;

namespace Urd.Services
{
    public class UnityServiceView : MonoBehaviour
    {
        IUnityService UnityService => StaticServiceLocator.Get<IUnityService>();

        private void OnApplicationFocus(bool focus)
        {
            UnityService?.CallOnGameFocus(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            UnityService?.CallOnGamePaused(pause);
        }
        private void OnApplicationQuit() => OnApplicationPause(true);
    }
}