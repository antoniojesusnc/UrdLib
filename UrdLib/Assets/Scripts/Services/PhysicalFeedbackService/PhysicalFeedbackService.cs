using System;
using UnityEngine;
using Urd.Feedback;

namespace Urd.Services
{
    [Serializable]
    public class PhysicalFeedbackService : BaseService, IPhysicalFeedbackService
    {
        public override int LoadPriority => 100;

        [SerializeReference, SubclassSelector]
        private IHapticProvider _provider;
        
        private IVibrationPlatform _vibrationPlatform;

        [field: SerializeField]
        public bool IsEnabled { get; private set; }
        
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
        public void Haptic(HapticType hapticType)
        {
            if (IsEnabled && _provider != null)
            {
                _provider.Haptic(hapticType);
            }
        }

        public void Vibrate(VibrationType vibrationType)
        {
            if (IsEnabled)
            {
                _vibrationPlatform.Vibrate(vibrationType);
            }
        }

        public void VibratePattern(int loop = -1, params long[] pattern)
        {
            if (IsEnabled)
            {
                _vibrationPlatform.VibratePattern(loop, pattern);
            }
        }

        public void Cancel() => _vibrationPlatform.Cancel();
        public void SetHapticEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
