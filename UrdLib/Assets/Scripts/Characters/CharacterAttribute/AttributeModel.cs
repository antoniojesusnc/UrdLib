using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

namespace Urd.Character
{
    [Serializable]
    public class AttributeModel : IDisposable
    {
        [JsonIgnore()]
        [field: SerializeField]
        public CharacterAttributeType CharacterAttributeType { get; private set; }

        [JsonIgnore()]
        [field: SerializeField, JsonIgnore]
        public double Max { get; private set; }
        [JsonIgnore()]
        public double RawCurrent => CalculateCurrent(false);
        [JsonIgnore()]
        public double Current => CalculateCurrent();
        
        [field: SerializeField]
        public double DeltaPercentage01 { get; private set; }
        
        [JsonIgnore()]
        public float Factor => (float)(Current / MaxWithModifiers());
        [JsonIgnore()]
        public bool IsPositive => Current > 0;
        [JsonIgnore()]
        public bool IsFull => Current >= MaxWithModifiers();

        [JsonIgnore()]
        public List<ICharacterAttributeModifier> AttributesModifiers = new List<ICharacterAttributeModifier>(); 
        
        public event Action<double> OnChangedCurrent;
        
        [JsonProperty("initial_value")]
        private double _initialValue;
        [JsonProperty("initial_max_value")]
        private double _initialMaxValue;
        [JsonProperty("raw_current")]
        private double _rawCurrent;

        public AttributeModel() : this(default, double.MaxValue, 0, 0) { }
        public AttributeModel(double maxValue, double initialValue, double deltaPercentage01 = 0) : this(default, maxValue, initialValue, deltaPercentage01) { }
        public AttributeModel(CharacterAttributeType attributeType, double maxValue, double deltaPercentage01 = 0) : this(attributeType, maxValue, maxValue, deltaPercentage01) { }
        
        public AttributeModel(CharacterAttributeType attributeType, double maxValue, double initialValue, double deltaPercentage01Value)
        {
            CharacterAttributeType = attributeType;
            Max = maxValue;
            _initialMaxValue = maxValue;
            _rawCurrent = initialValue;
            _initialValue = initialValue;
            DeltaPercentage01 = deltaPercentage01Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modification"></param>
        /// <returns>Clamped Value of the modification</returns>
        public double Add(double modification) => Modify(Math.Clamp(modification, 0, MaxWithModifiers()));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modification"></param>
        /// <returns>Clamped Value of the modification</returns>
        public double Deduct(double modification) => Modify(-Math.Clamp(modification, 0, MaxWithModifiers()));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modification"></param>
        /// <returns>Clamped Value of the modification</returns>
        public double Set(double value)
        {
            _rawCurrent = 0;
            return Modify(Math.Clamp(value, 0, MaxWithModifiers()));
        }

        public double Modify(double modification)
        {
            _rawCurrent += modification;
            
            if (modification != 0)
            {
                OnChangedCurrent?.Invoke(Current);
            }
            return modification;
        }

        public void Dispose()
        {
            OnChangedCurrent = null;
            AttributesModifiers.Clear();
        }

        public void AddModifier(ICharacterAttributeModifier attributeModifier)
        {
            if (!AttributesModifiers.Exists(modifier => modifier.Id == attributeModifier.Id))
            {
                AttributesModifiers.Add(attributeModifier);
                OnChangedCurrent?.Invoke(Current);
            }
        }

        public void RemoveModifier(ICharacterAttributeModifier attributeModifier)
        {
            string id = attributeModifier.Id;
            var index = AttributesModifiers.FindIndex(modifier => modifier.Id == id);
            if (index < 0)
            {
                return;
            }

            AttributesModifiers.RemoveAt(index);
            OnChangedCurrent?.Invoke(Current);
        }

        private double CalculateCurrent(bool addDelta = true)
        {
            var current = _rawCurrent;
            for (int i = 0; i < AttributesModifiers.Count; i++)
            {
                current = AttributesModifiers[i].ModifyAttribute(current);
            }

            if (addDelta && DeltaPercentage01 > 0)
            {
                Random random = new Random();
                var minimum = -DeltaPercentage01 * current;
                var maximum = DeltaPercentage01 * current;
                var modification = random.NextDouble() * (maximum - minimum) + minimum;
                current += modification;
            }
            return current;
        }

        public double MaxWithModifiers()
        {
            double max = Max;
            foreach (var modifier in AttributesModifiers) 
                max = modifier.ModifyAttribute(max);
            return max;
        }

        public void Reset()
        {
            Max = _initialMaxValue;
            AttributesModifiers.Clear();
            Set(_initialValue);
        }
        
        public void Initialize(double initialMaxValue)
        {
            _initialMaxValue = initialMaxValue;
            _initialValue = _initialMaxValue;
            SetMax((float)initialMaxValue);
        }

        public void SetMax(float newMax, bool updateCurrent = true)
        {
            Max = newMax;
            if (updateCurrent)
            {
                Set(Max);
            }
        }
    }
}
