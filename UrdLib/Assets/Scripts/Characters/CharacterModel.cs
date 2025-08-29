using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Urd.Utils;
using UnityEngine;

namespace Urd.Character
{
    [Serializable]
    public class CharacterModel<T> : ICharacterModel where T : CharacterConfig
    {
        private static int AUTO_ID;
        
        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonIgnore]
        public int Id { get; private set; }

        [JsonProperty("level")]
        public AttributeModel Level { get; private set; } =
            new AttributeModel(CharacterAttributeType.Level, int.MaxValue, 1, 0 );
        
        [JsonIgnore]
        public T Config { get; private set; }

        [JsonIgnore]
        public Sprite Sprite => Config.IdleImage;
        
        [JsonIgnore]
        private List<AttributeModel> _attributes = new List<AttributeModel>();

        public CharacterModel()
        {
            Id = AUTO_ID++;
        }
        
        public virtual void Dispose()
        {
            _attributes.ForEach(attibrute => attibrute.Dispose());
            _attributes.Clear();
        }
        
        public void SetConfig<T1>(T1 config)
        {
            Config = config as T;
            Name = Config.name;
        
            InitAttributes();
        }

        public virtual void InitAttributes()
        {
            SetLevel(Level.Current.RoundToInt());
        }

        public virtual void SetLevel(int level)
        {
            Level.Set(level);
        }
        
        public T1 GetProgressionPerLevel<T1>(float level = -1) where T1 : class, IProgressionData
        {
            if (level <= -1)
            {
                level = Level.Current.RoundToInt();
            }
            var levelProgression = Config.Progression.Find(info => info.Level == level) as T1;
            return levelProgression;
        }

        public AttributeModel AddAttribute(CharacterAttributeType type, float maxValue,
            float initialValue = float.MinValue, float deltaPercentage01 = 0)
        {
            var initial = Mathf.Approximately(initialValue, float.MinValue) ? maxValue : initialValue;
            var attributeModel = new AttributeModel(type, maxValue, initial, deltaPercentage01);
            _attributes.Add(attributeModel);
            return attributeModel;
        }

        public virtual void ResetStats()
        {
            _attributes.Clear();
            SetConfig(Config);
        }
    }
}