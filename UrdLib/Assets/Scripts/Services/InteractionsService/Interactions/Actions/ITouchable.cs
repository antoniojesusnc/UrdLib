using UnityEngine;

namespace Urd.Inputs
{
    public interface ITouchable : IInteractable
    {
        void OnTouch(Vector2 position);
    }
}
