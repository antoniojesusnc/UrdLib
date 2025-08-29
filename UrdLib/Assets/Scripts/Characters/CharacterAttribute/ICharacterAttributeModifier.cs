namespace Urd.Character
{
    public interface ICharacterAttributeModifier
    {
        public string Id { get; }
        public CharacterAttributeType AttributeType { get; }
        public float Amount { get; }
        public string CustomTextFormatAmountText { get; }
        string Text { get; }

        double ModifyAttribute(double characterAttributeModel);
    }
}
