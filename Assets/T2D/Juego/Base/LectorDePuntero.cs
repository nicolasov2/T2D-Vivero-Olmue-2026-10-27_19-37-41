using UnityEngine;
#if ENABLE_INPUT_SYSTEM && T2D_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace T2D.Juego
{
    /// <summary>
    /// The only place that reads the mouse or touch. Works with the Input System package and with the
    /// old Input Manager, whichever the project has active.
    /// </summary>
    public static class LectorDePuntero
    {
        /// <summary>Pointer position in screen pixels (origin bottom-left).</summary>
        public static Vector2 PosicionEnPantalla()
        {
#if ENABLE_INPUT_SYSTEM && T2D_INPUT_SYSTEM
            return Pointer.current == null ? Vector2.zero : Pointer.current.position.ReadValue();
#else
            return Input.mousePosition;
#endif
        }

        /// <summary>True on the frame the main button or a finger goes down.</summary>
        public static bool SePresionoEsteFrame()
        {
#if ENABLE_INPUT_SYSTEM && T2D_INPUT_SYSTEM
            return Pointer.current != null && Pointer.current.press.wasPressedThisFrame;
#else
            return Input.GetMouseButtonDown(0);
#endif
        }
    }
}
