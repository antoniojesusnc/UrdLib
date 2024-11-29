using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    [RequireComponent(typeof(Image))]
    public class UILineRenderer : MonoBehaviour
    {
        [SerializeField] private float _lineWidth = 10;
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _point2;
        
        private RectTransform _imageRectTransform;

        private void Awake()
        {
            _imageRectTransform = GetComponent<RectTransform>();
        }

        public void SetPoints(Transform point1, Transform point2, float lineWidth = 10)
        {
            _lineWidth = lineWidth;
            _point1 = point1;
            _point2 = point2;
            Calculate();
        }

        void Update()
        {
            Calculate();
        }
        
        [ButtonMethod]
        private void Calculate()
        {
            if (_point1 == null || _point2 == null || _imageRectTransform == null)
            {
                return;
            }

            Vector2 finalPos01 = _point1.position.ToVector2() - Vector2.up * (100 * transform.lossyScale.y);
            Vector2 finalPos02 = _point2.position.ToVector2() - Vector2.up * (100 * transform.lossyScale.y);
            
            Vector3 differenceVector = finalPos02 - finalPos01;
            
            _imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude/transform.lossyScale.y, _lineWidth);
            _imageRectTransform.pivot = new Vector2(0, 0.5f);
            _imageRectTransform.position = finalPos01;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            _imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}