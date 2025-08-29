using System;

namespace Urd.Character
{
    [Serializable]
    public class CharacterAttributeAdd : CharacterAttributeModifier
    {
        public override string Text => $"+{Amount} {AttributeType.ToReadableString()}";

        public CharacterAttributeAdd(string id, CharacterAttributeType attributeType, float amount): base(id, attributeType, amount) { }
        
        public override double ModifyAttribute(double characterAttributeModel)
        {
            return characterAttributeModel + Amount;
        }
    }
}
