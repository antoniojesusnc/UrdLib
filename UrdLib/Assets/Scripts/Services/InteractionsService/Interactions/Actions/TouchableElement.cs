using UnityEngine;
using UnityEngine.Events;

namespace Urd.Inputs
{
    [RequireComponent(typeof(Collider2D))]
    public class TouchableElement : MonoBehaviour, ITouchable
    {
        [SerializeField]
        private UnityEvent onTouchAction;
        public bool IsInteractable => true;

        public void OnTouch(Vector2 position)
        {
            onTouchAction?.Invoke();
        }

    }
}