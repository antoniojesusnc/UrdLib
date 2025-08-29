using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Urd.Character;
using Urd.Utils;
using UnityEngine;

namespace Urd
{
    [Serializable]
    public class PlayerModel<T> : CharacterModel<T>, IPlayerModel where T : CharacterConfig
    {
        [JsonProperty("world")] public AttributeModel WorldLevel { get; private set; }
        [JsonProperty("settings")] public PlayerSettingsModel SettingsModel { get; private set; }

        [JsonProperty("tutorials_finished")] public HashSet<string> TutorialsCompleted { get; private set; } = new();
        
        [SerializeField, JsonProperty("wallet")]
        private PlayerWallet _wallet = new();
        public IPlayerWallet Wallet => _wallet;
        

        [JsonIgnore]
        public bool IsMaxLevel => Level.IsFull;

        public void Init()
        {
            
        }

        public PlayerModel() : base()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _wallet?.Dispose();
            WorldLevel?.Dispose();
            SettingsModel?.Dispose();
        }

        public new void SetConfig<T1>(T1 config) where T1 : CharacterConfig
        {
            base.SetConfig(config);
        }

        public void SetPlayerSettingsModel(PlayerSettingsModel playerSettingsModel)
        {
            SettingsModel = playerSettingsModel;
        }


        public override void InitAttributes()
        {
            Level.SetMax(Mathf.Clamp(Config.Progression.Count,1,int.MaxValue), false);
            WorldLevel ??= AddAttribute(CharacterAttributeType.Level, int.MaxValue, 1);
            base.InitAttributes();
        }

        public void IncreaseLevel()
        {
            Level.Add(1);
            SetLevel(Level.Current.RoundToInt());
        }
    }
}