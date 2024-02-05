using MyBox;
using UnityEngine;
using Urd.Utils;

namespace Urd.Inputs
{
    public class CameraClickability : MonoBehaviour
    {
        private Camera _camera;

        private InputTouchController _inputTouchController;
        private IDraggable _dragObject;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _inputTouchController = new InputTouchController();
            
            SetClickablity(true);
        }

        public void SetClickablity(bool enableClick)
        {
            if (enableClick)
            {
                Subscribe();
                
            }
            else
            {
                UnSubscribe();
            }
        }

        private void Subscribe()
        {
            _inputTouchController.OnClick += OnClick;
            _inputTouchController.OnDrag += OnDrag;
        }

        private void UnSubscribe()
        {
            if (_inputTouchController != null)
            {
                _inputTouchController.OnClick -= OnClick;
                _inputTouchController.OnDrag -= OnDrag;
            }
        }

        private void OnDestroy()
        {
            UnSubscribe();
            _inputTouchController?.Dispose();
            _inputTouchController = null;
        }

        private bool TryGetClickElement<T>(Vector2 position, out T result) where T : class
        {
            result = null;
            var ray = _camera.ScreenPointToRay(position);
            var hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 100, LayerUtils.Interactable);
            if(hitInfo.transform != null)
            {
                result = hitInfo.transform.GetComponentInParent<T>();
                return result != null;
            }

            return false;
        }
        
        private void OnClick(Vector2 position)
        {
            if (TryGetClickElement(position, out ITouchable touchable))
            {
                touchable.OnTouch();
            }
        }


        private void OnDrag(bool isDragging, Vector2 position)
        {
            if (!TryGetClickElement(position, out IDraggable dragCandidate))
            {
                return;
            }

            var worldPosition = _camera.ScreenToWorldPoint(position).SetZ(0);

            if (!isDragging)
            {
                _dragObject?.OnEndDrag(worldPosition);
                _dragObject = null;
            }
            else if (_dragObject == null)
            {
                _dragObject = dragCandidate;
                _dragObject.OnBeginDrag(worldPosition);
            }
            else if (_dragObject == dragCandidate)
            {
                _dragObject.OnDrag(worldPosition);
            }
            else
            {
                _dragObject.OnEndDrag(worldPosition);
                _dragObject = dragCandidate;
                _dragObject.OnBeginDrag(worldPosition);
            }
        }
    }
}
