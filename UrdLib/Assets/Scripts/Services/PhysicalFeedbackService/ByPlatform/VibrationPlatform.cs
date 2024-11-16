namespace Urd.Feedback
{
    public abstract class VibrationPlatform : IVibrationPlatform
    {
        public virtual void Vibrate(VibrationType vibrationType)
        {
#if UNITY_ANDROID || UNITY_IOS

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
#endif
        }

        public virtual void Cancel() { }

        public virtual void VibratePattern(int loop = -1, params long[] vibrationType) { }
    }
}