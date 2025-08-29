using System;
using UnityEngine;

namespace Urd.Character
{
    [Serializable]
    public class ProgressionValueCostData : ProgressionData
    {
        [field: SerializeField]
        public float Value { get; protected set; }
        
        [field: SerializeField]
        public double Cost { get; protected set; }

        public ProgressionValueCostData(int level, float value, double cost) : base(level)
        {
            Value = value;
            Cost = cost;
        }
    }
}
