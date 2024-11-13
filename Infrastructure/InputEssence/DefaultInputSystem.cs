using UnityEngine;

namespace Infrastructure.InputEssence
{
    public class DefaultInputSystem : IInputSystem
    {
        public Vector3 MousePosition => CalculatePosition();

        private Vector3 CalculatePosition()
        {
#if ENABLE_LEGACY_INPUT_MANAGER == false
            #if UNITY_EDITOR
            return Mouse.current.position.ReadUnprocessedValue();
            #endif
            return Touchscreen.current.position.ReadUnprocessedValue();
#else
            return Input.mousePosition;
#endif
        }
    }
}