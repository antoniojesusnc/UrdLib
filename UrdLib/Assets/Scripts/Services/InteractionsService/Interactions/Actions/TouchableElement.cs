using UnityEngine;
using UnityEngine.Events;

namespace Urd.Inputs
{
    [RequireComponent(typeof(Collider2D))]
    public class TouchableElement : MonoBehaviour, ITouchable
    {
        [SerializeField]
        private UnityEvent onTouchAction;

        public void OnTouch()
        {
            onTouchAction?.Invoke();
        }
    }
}