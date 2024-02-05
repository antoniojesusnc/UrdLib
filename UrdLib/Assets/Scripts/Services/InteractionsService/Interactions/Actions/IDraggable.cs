using UnityEngine;

namespace Urd.Inputs
{
    public interface IDraggable
    {
        void OnBeginDrag(Vector2 position);
        void OnDrag(Vector2 position);
        void OnEndDrag(Vector2 position);
    }
}
