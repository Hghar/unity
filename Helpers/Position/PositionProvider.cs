using UnityEngine;

namespace Helpers.Position
{
    public class PositionProvider : MonoBehaviour, IPosition
    {
        [SerializeField] private Transform _target;

        public Vector2 WorldPosition => Get();

        public Vector2 Get() => _target.position;
        public void Set(Vector2 value) => _target.position = value;
        public void InvokeCache()
        {
            throw new System.NotImplementedException();
        }
    }
}