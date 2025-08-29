using System;
using UnityEngine;

namespace Urd.Character
{
    [Serializable]
    public abstract class CharacterAttributeModifier : ICharacterAttributeModifier
    {
        [field: SerializeField] public string Id { get; private set; }

        [field: SerializeField]
        public CharacterAttributeType AttributeType { get; private set; }
        
        [field: SerializeField]
        public float Amount { get; private set; }

        [field: SerializeField, Tooltip("amount value, e.g.: +{0}% {1}")]
        public string CustomTextFormatAmountText { get; private set; }

        public CharacterAttributeModifier() { }

        protected CharacterAttributeModifier(string id, CharacterAttributeType attributeType, float amount)
        {
            Id = id;
            AttributeType = attributeType;
            Amount = amount;
        }

        public virtual string Text => string.IsNullOrEmpty(CustomTextFormatAmountText)
            ? string.Format(CustomTextFormatAmountText, Amount, AttributeType.ToReadableString())
            : string.Empty;
        
        public abstract double ModifyAttribute(double characterAttributeModel);
    }
}
