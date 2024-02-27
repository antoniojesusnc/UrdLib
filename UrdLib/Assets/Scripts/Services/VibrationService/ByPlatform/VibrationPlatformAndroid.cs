using Urd.Services;

namespace Urd.Vibrate
{
    public class VibrationPlatformAndroid : VibrationPlatform
    {
#if UNITY_ANDROID
        private readonly long[] RIGID_DURATION = new long[]{0, 2000};
        private readonly long[] SOFT_DURATION = new long[]{0, 50, 50, 50 };
        private readonly long[] ERROR_DURATION = new long[]{0, 1000, 500, 2000};
        private readonly long[] SUCCESS_DURATION = new long[]{0, 100, 100, 100, 100,100, 200, 1000};
        private readonly long[] WARNING_DURATION = new long[]{0, 200, 100, 200, 100, 200};
        public override void Vibrate(VibrationType vibrationType)
        {
            base.Vibrate(vibrationType);
            
            switch (vibrationType)
            {
                case VibrationType.Rigid: VibratePattern(pattern:RIGID_DURATION);
                    break;
                case VibrationType.Soft:VibratePattern(pattern:SOFT_DURATION);
                    break;
                case VibrationType.Error:VibratePattern(pattern:ERROR_DURATION);
                    break;
                case VibrationType.Success:VibratePattern(pattern:SUCCESS_DURATION);
                    break;
                case VibrationType.Warning:VibratePattern(pattern:WARNING_DURATION);
                    break;
                default:
                    Vibration.Vibrate();
                    break;
            }
        }

        public override void Cancel()
        {
            Vibration.CancelAndroid();
        }

        public override void VibratePattern(int loop = -1, params long[] pattern) 
        {
            Vibration.VibrateAndroid ( pattern, loop);
        }
#endif
    }
}