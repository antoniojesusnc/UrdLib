using System;
using Urd.Audio;

namespace Urd.Services
{
    public interface IAudioService : IBaseService
    {
        public void SetConfig(AudioConfig audioConfig);

        public void PlaySound(AudioModel audioModel);
        public void PlaySound(Enum audioType);
        public bool IsSoundOfType(Enum audioType);
        public bool IsSoundOfType(AudioModel audioModel);
        public void StopSound(Enum audioType, Action onStopSound);
        public void StopSound(AudioModel audioModel, Action onStopSound);
    }
}
