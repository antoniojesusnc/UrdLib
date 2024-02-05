using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Urd.Services;

namespace Urd.Inputs
{
    public class InputTouchController : IDisposable
    {
        private TouchInputs _touchInputs;

        private IInputTouch _inputTouch;

        public event Action<Vector2> OnClick;
        public event Action<bool, Vector2> OnDrag;

        private IClockService _clockService;

        private bool _isDragging;
        
        public InputTouchController()
        {
            _clockService = StaticServiceLocator.Get<IClockService>();
                
            _touchInputs = new TouchInputs();

            InitInputTouch();
            Enable();
        }

        private void InitInputTouch()
        {
            _inputTouch = new InputTouchTouchScreen();
        }

        public void Dispose()
        {
            Disable();
            _touchInputs?.Dispose();
            _touchInputs = null;
        }
        
        public void Enable()
        {
            _clockService.SubscribeToUpdate(CustomUpdate);
            
            _touchInputs.Enable();
            _touchInputs.UI.Click.performed += OnClickOnScreen;
            
            _touchInputs.UI.DragAndMove.performed += OnBeginDrag;
            _touchInputs.UI.DragAndMove.canceled += OnFinishDrag;
        }
        
        public void Disable()
        {
            _clockService?.UnSubscribeToUpdate(CustomUpdate);
            
            _touchInputs.Disable();
            _touchInputs.UI.Click.performed -= OnClickOnScreen;
            
            _touchInputs.UI.DragAndMove.performed -= OnBeginDrag;
            _touchInputs.UI.DragAndMove.canceled -= OnFinishDrag;
        }

        private void CustomUpdate(float delta)
        {
            if (_isDragging)
            {
                OnDrag?.Invoke(true, _inputTouch.ScreenPosition);
            }
        }
        
        private void OnClickOnScreen(InputAction.CallbackContext context)
        {
            OnClick?.Invoke(_inputTouch.ScreenPosition);
        }
        
        private void OnBeginDrag(InputAction.CallbackContext context)
        {
            OnDrag?.Invoke(true, _inputTouch.ScreenPosition);
            _isDragging = true;
        }
        
        private void OnFinishDrag(InputAction.CallbackContext context)
        {
            OnDrag?.Invoke(false, _inputTouch.ScreenPosition);
            _isDragging = false;
        }
    }
}
