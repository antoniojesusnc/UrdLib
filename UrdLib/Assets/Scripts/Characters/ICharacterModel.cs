using System;

namespace Urd.Character
{
    public interface ICharacterModel : IDisposable
    {
        void SetConfig<T>(T config);
        void InitAttributes();
        AttributeModel Level { get; }

        AttributeModel AddAttribute(CharacterAttributeType type, float maxValue,
            float initialValue = float.MinValue, float deltaPercentage01 = 0);
    }
}