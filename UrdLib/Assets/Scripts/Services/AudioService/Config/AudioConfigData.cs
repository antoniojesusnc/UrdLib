using System;
using UnityEngine;
using Urd.Audio;

namespace Urd.Services
{
    [Serializable]
    public abstract class AudioConfigData<T> : IAudioConfigData where T : Enum
    {
        public Enum Type => AudioType;
        
        [field: SerializeField]
        public T AudioType { get; private set; }
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0,1)] public float Volume { get; private set; } = 1;
        [field: SerializeField] public AudioMixerType Mixer { get; private set; } = AudioMixerType.Sfx;
        [field: SerializeField, Range(-5,5)] public float Pitch { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
    }
}