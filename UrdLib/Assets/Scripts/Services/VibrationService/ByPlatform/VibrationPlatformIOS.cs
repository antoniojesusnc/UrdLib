using System;
using DG.Tweening;
using Urd.Services;

namespace Urd.Vibrate
{
    public class VibrationPlatformIOS : VibrationPlatform
    {
#if UNITY_IOS

        private const float LIGHT_THRESHOLD = 100;
        private const float MEDIUM_THRESHOLD = 500;

        public override void Vibrate(VibrationType vibrationType)
        {
            base.Vibrate(vibrationType);
            
            switch (vibrationType)
            {
                case VibrationType.Rigid:Vibration.VibrateIOS(ImpactFeedbackStyle.Rigid);
                    break;
                case VibrationType.Soft:Vibration.VibrateIOS(ImpactFeedbackStyle.Soft);
                    break;
                case VibrationType.Error: Vibration.VibrateIOS(NotificationFeedbackStyle.Error);
                    break;
                case VibrationType.Success:Vibration.VibrateIOS(NotificationFeedbackStyle.Success);
                    break;
                case VibrationType.Warning:Vibration.VibrateIOS(NotificationFeedbackStyle.Warning);
                    break;
            }
        }

        public override void VibratePattern(int loop = -1, params long[] pattern)
        {
            if (pattern == null || pattern?.Length <= 0)
            {
                return;
            }
            
            var sequence = DOTween.Sequence();
            for (int i = 0; i < pattern.Length; i++)
            {
                sequence.AppendInterval(pattern[i++]);
                if (pattern.Length > i)
                {
                    sequence.AppendCallback(() => Vibrate(GetVibrationByIntensity(pattern[i])));
                }
            }

            if (loop > 0)
            {
                sequence.SetLoops(loop);
            }
        }

        private VibrationType GetVibrationByIntensity(long duration)
        {
            if (duration < LIGHT_THRESHOLD)
            {
                return VibrationType.Light;
            }
            else if (duration < MEDIUM_THRESHOLD)
            {
                return VibrationType.Medium;
            }
            else
            {
                return VibrationType.Heavy;
            }
        }
#endif
    }
}