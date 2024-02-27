using Urd.Services;

namespace Urd.Vibrate
{
    public interface IVibrationPlatform
    {
        void Vibrate(VibrationType vibrationType);
        void Cancel();
        void VibratePattern(int loop = -1, params long[] pattern);
    }
}