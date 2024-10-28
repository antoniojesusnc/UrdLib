using Urd.Services;

namespace Urd.Feedback
{
    public interface IVibrationPlatform
    {
        void Vibrate(VibrationType vibrationType);
        void Cancel();
        void VibratePattern(int loop = -1, params long[] pattern);
    }
}