using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Urd.Utils;

namespace Urd.Inputs
{
    public class CameraClickability : MonoBehaviour
    {
        private Camera _camera;

        private InputTouchController _inputTouchController;
        private IDraggable _dragObject;
        private PointerEventData _pointerEventData;
        private List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private void Awake()
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
            if (_inputTouchController != null)
            {
                _inputTouchController.OnClick += OnClick;
                _inputTouchController.OnDrag += OnDrag;
            }
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

        private bool TryGetClickElement<T>(Vector2 position, out T result) where T : IInteractable
        {
            result = default;
            var ray = _camera.ScreenPointToRay(position);
            var hitInfo = Physics2D.RaycastAll(ray.origin, ray.direction, 100, LayerUtils.Interactable);
            for (int i = 0; i < hitInfo.Length; i++)
            {
                if (hitInfo[i].transform != null)
                {
                    result = hitInfo[i].transform.GetComponentInParent<T>();
                    if (result?.IsInteractable == true)
                    {
                        return result != null;
                    }
                }
            }

            return false;
        }
        
        private bool IsOverInteractableUI(Vector2 screenPosition)
        {
            _pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };
            _raycastResults.Clear();
            GraphicRaycaster[] graphicRaycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (GraphicRaycaster graphicRaycaster in graphicRaycasters)
            {
                graphicRaycaster.Raycast(_pointerEventData, _raycastResults);
                foreach (RaycastResult result in _raycastResults)
                {
                    var uiSelectable = result.gameObject.GetComponent<Selectable>();
                    var graphic = result.gameObject.GetComponent<Graphic>();
                    if (uiSelectable != null && uiSelectable.interactable
                        || graphic != null && graphic.raycastTarget)
                        return true;
                }
            }

            return false;
        }
        
        private void OnClick(Vector2 position)
        {
            if (IsOverInteractableUI(position))
            {
                return;
            }
            if (TryGetClickElement(position, out ITouchable touchable))
            {
                touchable.OnTouch(position);
            }
        }

        private void OnDrag(bool isDragging, Vector2 position)
        {
            if (IsOverInteractableUI(position))
            {
                return;
            }
            
            if (!TryGetClickElement(position, out IDraggable dragCandidate))
            {
                return;
            }

            if (!isDragging)
            {
                _dragObject?.OnEndDrag(position);
                _dragObject = null;
            }
            else if (_dragObject == null)
            {
                _dragObject = dragCandidate;
                _dragObject.OnBeginDrag(position);
            }
            else if (_dragObject == dragCandidate)
            {
                _dragObject.OnDrag(position);
            }
            else
            {
                _dragObject.OnEndDrag(position);
                _dragObject = dragCandidate;
                _dragObject.OnBeginDrag(position);
            }
        }
    }
}
