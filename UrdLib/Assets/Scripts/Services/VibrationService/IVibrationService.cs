namespace Urd.Services
{
    public interface IVibrationService : IBaseService
    {
        void Vibrate(VibrationType vibrationType);
        
        /// <summary>
        /// Pattern is delay 1, vibration time 1, delay 2, vibration time 2 ...
        /// </summary>
        /// <param name="pattern"></param>
        void VibratePattern(int loop = -1, params long[] pattern);
        void Cancel();
    }
}
