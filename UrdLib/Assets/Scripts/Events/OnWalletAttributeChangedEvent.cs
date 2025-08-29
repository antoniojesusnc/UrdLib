using Urd.Character;
using Urd.Services.EventBus;
using UnityEngine;

namespace Urd.Events
{
    public class OnWalletAttributeChangedEvent : IEventBusMessage
    {
        public CharacterAttributeType AttributeType { get; private set; }
        public double Modification { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsOverUI { get; private set; }

        public OnWalletAttributeChangedEvent(CharacterAttributeType attributeType, double modification, Vector3 position, bool isOverUI = false)
        {
            AttributeType = attributeType;
            Modification = modification;
            Position = position;
            IsOverUI = isOverUI;
        }

    }
}
