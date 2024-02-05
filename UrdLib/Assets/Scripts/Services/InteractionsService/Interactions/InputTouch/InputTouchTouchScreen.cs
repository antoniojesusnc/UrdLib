using UnityEngine;
using UnityEngine.InputSystem;

namespace Urd.Inputs
{
    public class InputTouchTouchScreen : IInputTouch
    {
        public Vector2 ScreenPosition => Touchscreen.current?.position.value ?? Vector2.zero;
    }
}
