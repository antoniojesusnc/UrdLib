using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Urd/Services/Audio Config", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public List<AudioMixerData> Mixers { get; private set; } = new List<AudioMixerData>();
        
        [field: SerializeField, DisplayInspector]

        public List<AudioClipConfig> Audios { get; private set; } = new List<AudioClipConfig>();
        
        public AudioMixerGroup GetMixer(AudioMixerType mixerType)
        {
            return Mixers?.Find(mixer => mixer.MixerType == mixerType)?.Mixer;
        }
        public bool TryGetAudioData(AudioModel audioModel, out IAudioConfigData audioConfigData)
        {
            audioConfigData = Audios.Find(audioConfigData => audioConfigData.Audio.Type.Equals(audioModel.AudioType))?.Audio;
            return audioConfigData != null;
        }
    }
}