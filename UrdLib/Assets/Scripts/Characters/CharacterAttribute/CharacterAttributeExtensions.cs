namespace Urd.Character
{
    public static class CharacterAttributeExtensions 
    {
        public static string ToReadableString(this CharacterAttributeType attributeType)
        {
            return attributeType.ToString();
        }
    }
}