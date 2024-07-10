using System;
using UnityEngine;
using Urd.Services;

namespace Urd.Audio
{
    public class AudioModel : IDisposable
    {
        public Enum AudioType { get; private set; }
        public Transform AudioLocation { get; private set; } = null;
        public float FadeOut { get; private set; }
        private float _volume = int.MaxValue;

        public float Volume => _volume != int.MaxValue
            ? _volume
            : AudioConfigData.Volume;
        
        private float _pitch = int.MaxValue;

        public float Pitch => _pitch != int.MaxValue
            ? _pitch
            : AudioConfigData.Pitch;
        private AudioMixerType _audioMixerType = AudioMixerType.None;
        public AudioMixerType AudioMixerType => _audioMixerType != AudioMixerType.None 
            ? _audioMixerType
            : AudioConfigData.Mixer ;
        public IAudioConfigData AudioConfigData { get; private set; }
        public AudioClip _clip;
        public AudioClip Clip => _clip != null
            ? _clip
            : AudioConfigData.Clip;
        
        public bool Loop => AudioConfigData.Loop;

        public AudioModel(Enum audioType)
        {
            AudioType = audioType;
        }
        
        public void SetAudioClip(AudioClip audioClip)
        {
            _clip = audioClip;
        }

        public void SetAudioLocation(Transform audioLocation)
        {
            AudioLocation = audioLocation;
        }
        
        public void SetVolume(float volume)
        {
            _volume = volume;
        }
        
        public void SetToDefaultVolume()
        {
            _volume = int.MaxValue;
        }

        public void SetAudioMixerType(AudioMixerType audioMixerType)
        {
            _audioMixerType = audioMixerType;
        }
        
        public void SetFadeOut(float fadeOut)
        {
            FadeOut = fadeOut;
        }

        public void Dispose()
        {
        }

        public void SetAudioConfigData(IAudioConfigData audioConfigData)
        {
            AudioConfigData = audioConfigData;
        }

        public void SetPitch(float pitch)
        {
            _pitch = pitch;
        }
    }
}
