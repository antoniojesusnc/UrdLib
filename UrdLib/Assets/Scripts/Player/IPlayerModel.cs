using System;
using System.Collections.Generic;
using Urd.Character;
using Urd.Character;

namespace Urd
{
    public interface IPlayerModel : IDisposable
    {
        AttributeModel WorldLevel { get; }
        AttributeModel Level { get; }
        PlayerSettingsModel SettingsModel { get; }

        HashSet<string> TutorialsCompleted { get; }

        IPlayerWallet Wallet { get; }

        public bool IsMaxLevel { get; }

        void Init();
        void ResetStats();
        void IncreaseLevel();
        
        void SetConfig<T>(T config) where T : CharacterConfig;
        void SetPlayerSettingsModel(PlayerSettingsModel playerSettingsModel);
    }
}