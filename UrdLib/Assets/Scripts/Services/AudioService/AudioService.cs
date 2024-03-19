using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;
using Urd.Error;

namespace Urd.Services
{
    public class AudioService : BaseService, IAudioService
    {
        [SerializeField]
        private AudioConfig _audioConfig;

        private AudioServiceView _audioServiceView;
        
        public override int LoadPriority => 100;

        private List<AudioSource> _audioSources;
        
        public override void Init()
        {
            base.Init();
            
            GetAudioServiceView();
        }
        
        public void SetConfig(AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;
        }

        private void GetAudioServiceView()
        {
            _audioServiceView = GameObject.FindObjectOfType<AudioServiceView>();
        }

        public void PlaySound(Enum audioType) => PlaySound(new AudioModel(audioType));

        public void PlaySound(AudioModel audioModel)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioConfigData audioConfigData))
            {
                ErrorModel errorModel = new ErrorModel($"Audio with enum {audioModel.AudioType} not Found",
                                                       ErrorCode.Error_404_Not_Found);
                return;
            }

            audioModel.SetAudioConfigData(audioConfigData);
            PlayInternal(audioModel);
        }

        private void PlayInternal(AudioModel audioModel)
        {
            var audioSource = GetEmptyAudioSource(audioModel);
            audioSource.clip = audioModel.Clip;
            audioSource.volume = audioModel.Volume;
            audioSource.pitch = audioModel.Pitch;
            audioSource.loop = audioModel.Loop;
            audioSource.outputAudioMixerGroup = GetAudioMixer(audioModel);
            
            audioSource.Play();
        }

        private AudioMixerGroup GetAudioMixer(AudioModel audioModel)
        {
            return _audioConfig.GetMixer(audioModel.AudioMixerType);
        }

        private AudioSource GetEmptyAudioSource(AudioModel audioModel)
        {
            Transform audioSourceLocation = _audioServiceView.transform;
            if (audioModel.AudioLocation != null)
            {
                audioSourceLocation = audioModel.AudioLocation;
            }

            var audioSources = audioSourceLocation.GetComponents<AudioSource>()?.ToList() ?? new List<AudioSource>();
            var audioSource = audioSources.Find(audioSource => !audioSource.isPlaying);
            if (audioSource == null)
            {
                audioSource = audioSourceLocation.gameObject.AddComponent<AudioSource>();
            }
            
            return audioSource;
        }


        public bool IsSoundOfType(Enum audioType) => IsSoundOfType(new AudioModel(audioType));

        public bool IsSoundOfType(AudioModel audioModel)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioConfigData audioConfigData))
            {
                ErrorModel errorModel = new ErrorModel($"Audio with enum {audioModel.AudioType} not Found",
                                                       ErrorCode.Error_404_Not_Found);
                return false;
            }

            audioModel.SetAudioConfigData(audioConfigData);

            var audioSource = GetAudioSourceThatSounds(audioModel);
            return audioSource != null;
        }
        
        private AudioSource GetAudioSourceThatSounds(AudioModel audioModel)
        {
            Transform audioSourceLocation = _audioServiceView.transform;
            if (audioModel.AudioLocation != null)
            {
                audioSourceLocation = audioModel.AudioLocation;
            }

            var audioSources = audioSourceLocation.GetComponents<AudioSource>()?.ToList() ?? new List<AudioSource>();
            return audioSources.Find(audioSource => audioSource.isPlaying && audioSource.clip == audioModel.Clip);
        }

        public void StopSound(Enum audioType, Action onStopSound) => StopSound(new AudioModel(audioType), onStopSound);
        
        public void StopSound(AudioModel audioModel, Action onStopSound)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioConfigData audioConfigData))
            {
                ErrorModel errorModel = new ErrorModel($"Audio with enum {audioModel.AudioType} not Found",
                                                       ErrorCode.Error_404_Not_Found);
                onStopSound?.Invoke();
                return;
            }
            audioModel.SetAudioConfigData(audioConfigData);

            var audioSource = GetAudioSourceThatSounds(audioModel);
            if (audioSource == null)
            {
                onStopSound?.Invoke();
                return;
            }

            if (audioModel.FadeOut <= 0)
            {
                audioSource.Stop();
                onStopSound?.Invoke();
                return;
            }

            audioSource.DOFade(0, audioModel.FadeOut).onComplete += () => onStopSound?.Invoke();
        }
    }
}
