using System;
using UnityEngine;
using Urd.Vibrate;

namespace Urd.Services
{
    [Serializable]
    public class VibrationService : BaseService, IVibrationService
    {
        public override int LoadPriority => 100;

        private IVibrationPlatform _vibrationPlatform;

        public override void Init()
        {
            base.Init();

            _vibrationPlatform = GetVibrationPlatform();
            
            Vibration.Init();
        }

        private IVibrationPlatform GetVibrationPlatform()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return new VibrationPlatformAndroid();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return new VibrationPlatformIOS();
            }
            else
            {
                return new VibrationPlatformNone();
            }
        }

        public void Vibrate(VibrationType vibrationType) => _vibrationPlatform.Vibrate(vibrationType);
        
        public void VibratePattern(int loop = -1, params long[] pattern) => _vibrationPlatform.VibratePattern(loop, pattern);

        public void Cancel() => _vibrationPlatform.Cancel();
    }
}
