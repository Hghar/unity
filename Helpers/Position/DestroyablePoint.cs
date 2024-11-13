using System;
using UnityEngine;

namespace Helpers.Position
{
    public class DestroyablePoint : MonoBehaviour, IDestroyablePoint
    {
        private bool _isDestroying = false;

        public Vector2 WorldPosition => transform.position;
        public bool IsDestroying => _isDestroying;

        public event Action<IDestroyablePoint> Destroying;

        private void OnDestroy()
        {
            _isDestroying = true;
            Destroying?.Invoke(this);
        }

        public Vector2 Get() => WorldPosition;
    }
}