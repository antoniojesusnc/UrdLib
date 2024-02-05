using System;
using UnityEngine.InputSystem;

namespace Urd.Inputs
{
    public class InputKeysController : IDisposable
    {
        private KeysInput _keysInput;

        public event Action OnPressBack;
        
        public InputKeysController()
        {
            _keysInput = new KeysInput();
            
            Enable();
        }

        public void Dispose()
        {
            Disable();
            _keysInput?.Dispose();
        }
        
        
        public void Enable()
        {
            _keysInput.Enable();
            _keysInput.Keys.Back.performed += OnPressBackAction;
        }
        
        public void Disable()
        {
            _keysInput.Disable();
            _keysInput.Keys.Back.performed -= OnPressBackAction;
        }

        private void OnPressBackAction(InputAction.CallbackContext context)
        {
            OnPressBack?.Invoke();
        }

    }
}
