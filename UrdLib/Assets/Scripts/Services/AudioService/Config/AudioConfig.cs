using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Urd/Services/Audio Config", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public List<AudioMixerData> Mixers { get; private set; } = new List<AudioMixerData>();
        
        [field: SerializeReference, SubclassSelector]

        public List<IAudioConfigData> Audios { get; private set; } = new List<IAudioConfigData>();
        
        public AudioMixerGroup GetMixer(AudioMixerType mixerType)
        {
            return Mixers?.Find(mixer => mixer.MixerType == mixerType)?.Mixer;
        }
        public bool TryGetAudioData(AudioModel audioModel, out IAudioConfigData audioConfigData)
        {
            audioConfigData = Audios.Find(audioConfigData => audioConfigData.Type.Equals(audioModel.AudioType));
            return audioConfigData != null;
        }
    }
}