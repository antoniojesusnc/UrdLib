using System;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Urd.Utils.WorldMobile
{
    [ExecuteAlways]
    public class AdjustToBorders : MonoBehaviour
    {
        public enum Border
        {
            TopLeft,
            Top,
            TopRight,
            Left,
            Center,
            Right,
            BotLeft,
            Bot,
            BotRight
        }

        [SerializeField] private bool _executeInEditMode;

        [SerializeField] private Transform _objectToSetPosition;

        [SerializeField]
        private bool _adjustX = true;

        [SerializeField] private bool _adjustY = true;

        private Camera _camera;

        [SerializeField] private Border _border = Border.Center;
        
        [Range(-1000,1000)] [SerializeField] private float _XOffsetPosition = 0;
        [Range(-1000,1000)] [SerializeField] private float _yOffsetPosition = 0;
        
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
        
        void Update()
        {
#if UNITY_EDITOR
            if (!_executeInEditMode)
            {
                return;
            }
#endif
            if (_objectToSetPosition == null)
            {
                return;
            }

            Adjust();

        }

        private void Adjust()
        {
            switch (_border)
            {
                case Border.TopLeft: _objectToSetPosition.position = GetPosition(0, 1); break;
                case Border.Top: _objectToSetPosition.position = GetPosition(0.5f, 1); break;
                case Border.TopRight: _objectToSetPosition.position = GetPosition(1, 1); break;
                case Border.Left: _objectToSetPosition.position = GetPosition(0f, .5f); break;
                case Border.Center: _objectToSetPosition.position = GetPosition(0.5f, 0.5f); break;
                case Border.Right: _objectToSetPosition.position = GetPosition(1, 0.5f); break;
                case Border.BotLeft: _objectToSetPosition.position = GetPosition(0f, 0); break;
                case Border.Bot: _objectToSetPosition.position = GetPosition(0.5f, 0); break;
                case Border.BotRight: _objectToSetPosition.position = GetPosition(1, 0); break;
            }

            _objectToSetPosition.SetZ(0);
        }

        private Vector3 GetPosition(float x, float y)
        {
            var newPosition = Camera.ViewportToWorldPoint(new Vector2(x, y)) + new Vector3(_XOffsetPosition, _yOffsetPosition);
            return new Vector2(
                _adjustX ? newPosition.x : _objectToSetPosition.position.x,
                _adjustY ? newPosition.y : _objectToSetPosition.position.y);
        }
    }
}
