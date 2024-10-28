using System;
using UnityEngine.Audio;
using Urd.Services;

namespace Urd.Audio
{
    public class AudioMixerModel : IDisposable
    {
        public AudioMixerType Type => _audioMixerData.MixerType;

        public bool IsEnabled { get; private set; } = true;

        public AudioMixerGroup MixerGroup => _audioMixerData.Mixer;
        private AudioMixerData _audioMixerData;

        public event Action<bool> OnEnabledChanged;

        public AudioMixerModel(AudioMixerData audioMixerData)
        {
            _audioMixerData = audioMixerData;
        }
        
        public void Dispose()
        {
            
        }

        public void SetEnable(bool enable)
        {
            IsEnabled = enable;
            OnEnabledChanged?.Invoke(IsEnabled);
        }

        
    }
}