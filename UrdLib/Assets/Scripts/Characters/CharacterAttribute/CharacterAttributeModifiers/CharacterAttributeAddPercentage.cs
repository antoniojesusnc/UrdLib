using System;

namespace Urd.Character
{
    [Serializable]
    public class CharacterAttributeAddPercentage : CharacterAttributeModifier
    {
        public override string Text => $"+{Amount}% {AttributeType.ToReadableString()}";

        public CharacterAttributeAddPercentage() { }

        public CharacterAttributeAddPercentage(string id, CharacterAttributeType attributeType, float amount):
            base(id, attributeType, amount){}


        public override double ModifyAttribute(double characterAttributeModel)
        {
            return characterAttributeModel + Amount*0.01f;
        }
    }
}
