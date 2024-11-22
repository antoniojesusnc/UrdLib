using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    [RequireComponent(typeof(Image))]
    public class UILineRenderer : MonoBehaviour
    {
            [SerializeField]
            private float _lineWidth = 10;

            [SerializeField]
            private Vector2 _point1;
            
            [SerializeField]
            private Vector2 _point2;
            
            public void SetPoints(Vector2 point1, Vector2 point2, float lineWidth = 10)
            {
                _lineWidth = lineWidth;
                _point1 = point1;
                _point2 = point2;
                Calculate();
            }

            [ButtonMethod]
            private void Calculate()
            {
                Vector3 differenceVector = _point2 - _point1;

            var imageRectTransform = GetComponent<RectTransform>();
            imageRectTransform.sizeDelta = new Vector2( differenceVector.magnitude, _lineWidth);
            imageRectTransform.pivot = new Vector2(0, 0.5f);
            imageRectTransform.position = _point1;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            imageRectTransform.rotation = Quaternion.Euler(0,0, angle);
        }
    }
}