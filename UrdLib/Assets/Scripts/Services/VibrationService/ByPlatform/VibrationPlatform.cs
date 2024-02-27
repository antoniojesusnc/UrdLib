using Urd.Services;

namespace Urd.Vibrate
{
    public abstract class VibrationPlatform : IVibrationPlatform
    {
        public virtual void Vibrate(VibrationType vibrationType)
        {
            switch (vibrationType)
            {
                case VibrationType.Light: Vibration.VibratePop();
                    break;
                case VibrationType.Medium: Vibration.Vibrate();
                    break;
                case VibrationType.Heavy: Vibration.VibratePeek();
                    break;
                case VibrationType.ThreeTimes: Vibration.VibrateNope();
                    break;
                default:
                    Vibration.Vibrate();
                    break;
            }
        }

        public virtual void Cancel() { }

        public virtual void VibratePattern(int loop = -1, params long[] vibrationType) { }
    }
}