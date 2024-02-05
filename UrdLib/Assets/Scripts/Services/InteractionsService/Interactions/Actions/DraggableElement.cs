using UnityEngine;
using UnityEngine.Events;

namespace Urd.Inputs
{
    [RequireComponent(typeof(Collider2D))]
    public class DraggableElement : MonoBehaviour, IDraggable
    {
        [SerializeField]
        private UnityEvent<Vector2> onBeginDrag;
        [SerializeField]
        private UnityEvent<Vector2> onDrag;
        [SerializeField]
        private UnityEvent<Vector2> onEndDrag;

        public void OnBeginDrag(Vector2 position)
        {
            onBeginDrag?.Invoke(position);
        }

        public void OnDrag(Vector2 position)
        {
            onDrag?.Invoke(position);
        }

        public void OnEndDrag(Vector2 position)
        {
            onEndDrag?.Invoke(position);
        }
    }
}