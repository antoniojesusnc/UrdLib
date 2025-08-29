using System;

namespace Urd.Character
{
    [Serializable]
    public class CharacterAttributeIncreaseByPercentage : CharacterAttributeModifier
    {
        public CharacterAttributeIncreaseByPercentage() :base() 
        { }
        
        public CharacterAttributeIncreaseByPercentage(string id, CharacterAttributeType type, float amount): base(id, type, amount) 
        { }

        public override string Text => $"+{Amount}% {AttributeType.ToReadableString()}";
        
        public override double ModifyAttribute(double characterAttributeModel)
        {
            return characterAttributeModel + Amount/100f;
        }
    }
}
