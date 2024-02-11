using MyBox;
using UnityEngine;


namespace BrenaBurger
{
    [ExecuteAlways]
    public class AdjustToFullScreen : MonoBehaviour
    {
        [SerializeField] private bool _executeInEditMode;
        
        [SerializeField] private SpriteRenderer _imageToScale;

        [SerializeField]
        private bool _resizeX;
        [SerializeField]
        private bool _resizeY;
        
        [SerializeField][Header("Adjust")]
        private bool _relative;
        [Space]
        [ConditionalField("_relative")][Range(0,1)] [SerializeField] private float _topViewportOffset = 1;
        [ConditionalField("_relative")][Range(0,1)] [SerializeField] private float _botViewportOffset = 0;
        [ConditionalField("_relative")][Range(0,1)] [SerializeField] private float _rightViewportOffset = 1;
        [ConditionalField("_relative")][Range(0,1)] [SerializeField] private float _leftViewportOffset = 0;
        
        [ConditionalField("_relative", true)][Range(-100,100)] [SerializeField] private float _offsetLeft = 0;
        [ConditionalField("_relative", true)][Range(-100,100)] [SerializeField] private float _offsetRight = 0;
        [ConditionalField("_relative", true)][Range(-100,100)] [SerializeField] private float _offsetTop = 0;
        [ConditionalField("_relative", true)][Range(-100,100)] [SerializeField] private float _offsetBot = 0;

        private Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }

        private Camera _camera;
        
        private void Update()
        {
            #if UNITY_EDITOR
            if (!_executeInEditMode)
            {
                return;
            }
            #endif
            
            if (_imageToScale == null)
            {
                return;
            }

            if (_relative)
            {
                ResizeRelative();
            }
            else
            {
                ResizeAbsolute();
            }
        }

        private void ResizeAbsolute()
        {
            var maxValue = Camera.ViewportToWorldPoint(new Vector2(0, 0)) +
                           new Vector3(_offsetLeft, _offsetBot);
            var minValue = Camera.ViewportToWorldPoint(new Vector2(1, 1)) +
                           new Vector3(_offsetRight, _offsetTop);

            AdjustImageTo(minValue, maxValue);
        }

        private void ResizeRelative()
        {
            var maxValue = Camera.ViewportToWorldPoint(new Vector2(_topViewportOffset,_rightViewportOffset));
            var minValue = Camera.ViewportToWorldPoint(new Vector2(_botViewportOffset,_leftViewportOffset));
            AdjustImageTo(minValue, maxValue);

        }
        
        private void AdjustImageTo(Vector3 minValue, Vector3 maxValue)
        {
            var center = new Vector2((maxValue.x + minValue.x) * 0.5f, (maxValue.y + minValue.y) * 0.5f); 
            float width = maxValue.x - minValue.x;
            float height = maxValue.y - minValue.y;
            _imageToScale.size = new Vector2(_resizeX?width:_imageToScale.size.x, _resizeY?height:_imageToScale.size.y);
            _imageToScale.transform.position = new Vector3(_resizeX?center.x:_imageToScale.transform.position.x, _resizeY?center.y:_imageToScale.transform.position.y);
        }
    }
}