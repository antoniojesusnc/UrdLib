using UnityEngine;

namespace Urd
{
    public class TransformFollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        
        [SerializeField] private Vector3 _offset;

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }
        
        void Update()
        {
            _transform.position = _transform.position + _offset;
        }
    }
}