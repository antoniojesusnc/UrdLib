using UnityEngine;
using UnityEngine.InputSystem;

namespace Urd.Inputs
{
    public class InputTouchMouse : IInputTouch
    {
        public Vector2 ScreenPosition => Mouse.current?.position.value ?? Vector2.zero;
    }
}
