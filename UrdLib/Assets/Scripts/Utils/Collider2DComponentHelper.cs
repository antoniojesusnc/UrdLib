using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Urd.Utils
{
    public class Collider2DComponentHelper : MonoBehaviour
    {
        [SerializeField] 
        private LayerMask _layerMask;
        
        [Header("Triggers")]
        [SerializeField]
        private UnityEvent<Collider2D> onTriggerEnter2D;
        [SerializeField]
        private UnityEvent<Collider2D> onTriggerStay2D;
        [SerializeField]
        private UnityEvent<Collider2D> onTriggerExit2D;
        
        [Header("Colliders")]
        private UnityEvent<Collision2D> onCollisionExit2D;
        [SerializeField]
        private UnityEvent<Collision2D> onCollisionEnter2D;
        [SerializeField]
        private UnityEvent<Collision2D> onCollisionStay2D;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if ((_layerMask & 1 << collider2D.gameObject.layer) > 0)
            {
                onTriggerEnter2D?.Invoke(collider2D);
            }
        }

        private void OnTriggerStay2D(Collider2D collider2D)
        {
            if ((_layerMask & 1 << collider2D.gameObject.layer) > 0)
            {
                onTriggerStay2D?.Invoke(collider2D);
            }
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if ((_layerMask & 1 << collider2D.gameObject.layer) > 0)
            {
                onTriggerExit2D?.Invoke(collider2D);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if ((_layerMask & 1 << collision2D.gameObject.layer) > 0)
            {
                onCollisionEnter2D?.Invoke(collision2D);
            }
        }
        
        private void OnCollisionStay2D(Collision2D collision2D)
        {
            if ((_layerMask & 1 << collision2D.gameObject.layer) > 0)
            {
                onCollisionStay2D?.Invoke(collision2D);
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if ((_layerMask & 1 << collision2D.gameObject.layer) > 0)
            {
                onCollisionExit2D?.Invoke(collision2D);
            }
        }
    }
}