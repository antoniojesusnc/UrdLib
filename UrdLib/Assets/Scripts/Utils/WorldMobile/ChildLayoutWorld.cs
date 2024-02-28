using System;
using UnityEngine;

namespace Urd.Utils.WorldMobile
{
    [ExecuteAlways]
    public abstract class ChildLayoutWorld : MonoBehaviour
    {
        public enum Behavior
        {
            ExpandChilds,
            ChildSize,
        }
        
        [Header("Behavior")] 
        [SerializeField] 
        private Behavior _behavior;
        
        [Header("Size")] 
        [SerializeField]
        private float _size;
        
        [SerializeField]
        protected float _childSize;
        
        [Header("Padding")] 
        [SerializeField]
        protected float _space;

        protected abstract Vector3 Direction { get; }
        
        void Update()
        {
            if (transform.childCount <= 0)
            {
                return;
            }
            
            AdjustChildrens();
        }

        private void AdjustChildrens()
        {
            var initialPosition = transform.position;

            if (_behavior == Behavior.ChildSize)
            {
                AdjustChildrenSize(initialPosition);
            }
            else
            {
                AdjustExpandChild(initialPosition, _size);
            }
            
        }

        protected virtual void AdjustChildrenSize(Vector3 initialPosition)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.transform.position = initialPosition + Direction *_childSize * i + Direction*_space*i;
            }
        }

        private void AdjustExpandChild(Vector3 initialPosition, float size)
        {
            var childCount = transform.childCount;
            var finalSize = initialPosition + Direction * size;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                float factor = (float)(i)/(float)(childCount-1);
                if (childCount <= 1)
                {
                    factor = 0.5f;
                }else if (childCount == 2)
                {
                    factor = ((i*3)+1)/5f;
                }
                child.transform.position = Vector3.Lerp(initialPosition, finalSize,  factor);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position+Direction*_size);
        }
    }
}
