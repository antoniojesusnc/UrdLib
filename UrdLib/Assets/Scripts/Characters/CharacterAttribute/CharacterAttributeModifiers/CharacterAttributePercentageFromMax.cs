using System;

namespace Urd.Character
{
    [Serializable]
    public class CharacterAttributePercentageFromMax : CharacterAttributeModifier
    {
        public CharacterAttributePercentageFromMax() :base()
        {
            
        }
        public CharacterAttributePercentageFromMax(string id, CharacterAttributeType type, float amount) : base(id, type, amount)
        {
        }

        public override string Text => $"{AttributeType.ToReadableString()} {Amount}%";
        
        public override double ModifyAttribute(double characterAttributeModel)
        {
            return characterAttributeModel * (Amount/100f);
        }
    }
}
