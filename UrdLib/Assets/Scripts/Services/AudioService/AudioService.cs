using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Urd.Audio;
using Urd.Error;
using Object = UnityEngine.Object;

namespace Urd.Services
{
    [Serializable]
    public class AudioService : BaseService, IAudioService
    {
        [SerializeField]
        private AudioConfig _audioConfig;

        private AudioServiceView _audioServiceView;
        
        public override int LoadPriority => 100;

        private List<AudioMixerModel> _audioMixersModels = new List<AudioMixerModel>();
        private List<AudioSourceModel> _audioSourcesModels = new List<AudioSourceModel>();
        
        public override void Init()
        {
            base.Init();
            
            GetAudioServiceView();
            CreateMixerModels();
        }

        private void CreateMixerModels()
        {
            _audioMixersModels = new List<AudioMixerModel>();
            for (int i = 0; i < _audioConfig.Mixers.Count; i++)
            {
                var mixerModel = new AudioMixerModel(_audioConfig.Mixers[i]);
                mixerModel.OnEnabledChanged += isEnabled => OnMixerModelEnabledChanged(mixerModel);
                _audioMixersModels.Add(mixerModel);
            }
        }

        private void OnMixerModelEnabledChanged(AudioMixerModel mixerModel)
        {
            for (int i = 0; i < _audioSourcesModels.Count; i++)
            {
                var audioSource = _audioSourcesModels[i].AudioSource;
                if (audioSource.isPlaying && audioSource.outputAudioMixerGroup == mixerModel.MixerGroup)
                {
                    _audioSourcesModels[i].SetVolume(GetAudioMixer(_audioSourcesModels[i].AudioModel.AudioMixerType));
                }
            }
        }


        public void SetConfig(AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;
        }

        private void GetAudioServiceView()
        {
            _audioServiceView = Object.FindAnyObjectByType<AudioServiceView>();
        }

        public void PlaySound(Enum audioType) => PlaySound(new AudioModel(audioType));

        public void PlaySound(AudioModel audioModel)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioConfigData audioConfigData))
            {
                ErrorModel errorModel = new ErrorModel($"Audio with enum {audioModel.AudioType} not Found",
                                                       ErrorCode.Error_404_Not_Found);
                UnityEngine.Debug.LogWarning(errorModel.ToString());
                return;
            }

            audioModel.SetAudioConfigData(audioConfigData);
            PlayInternal(audioModel);
        }

        private void PlayInternal(AudioModel audioModel)
        {
            var audioSourceModel = GetAudioSourceModel(audioModel);
            if (audioSourceModel == null)
            {
                return;
            }
            
            audioSourceModel.SetAudioModel(audioModel);

            var audioSource = audioSourceModel.AudioSource;

            var mixer = GetAudioMixer(audioModel.AudioMixerType);
            
            audioSource.clip = audioModel.Clip;
            audioSource.pitch = audioModel.Pitch;
            audioSource.loop = audioModel.Loop;
            audioSource.outputAudioMixerGroup = mixer.MixerGroup;
            audioSourceModel.SetVolume(mixer);
            
            audioSource.Play();
        }

        public AudioMixerModel GetAudioMixer(AudioMixerType audioMixerType)
        {
            return _audioMixersModels.Find(model => model.Type == audioMixerType);
        }

        private AudioSourceModel GetAudioSourceModel(AudioModel audioModel)
        {
            AudioSourceModel audioSourceModel = null;

            if (_audioServiceView == null)
            {
                return null;
            }
            
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
                audioSourceModel = new AudioSourceModel(audioSource);
                _audioSourcesModels.Add(audioSourceModel);
            }
            else
            {
                audioSourceModel = _audioSourcesModels.Find(model => model.AudioSource == audioSource);
            }
            
            return audioSourceModel;
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

            audioSource.DOFade(0, audioModel.FadeOut).onComplete += () => OnFinishFadeOut(audioModel, audioSource,  onStopSound);
        }

        private void OnFinishFadeOut(AudioModel audioModel, AudioSource audioSource, Action onStopSound)
        {
            audioSource.Stop();
            onStopSound?.Invoke();
        }
    }
}
