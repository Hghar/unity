using UnityEngine;

namespace Infrastructure.InputEssence
{
    public interface IInputSystem
    {
        Vector3 MousePosition { get; }
    }
}