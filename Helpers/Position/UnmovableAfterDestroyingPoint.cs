using System;
using UnityEngine;

namespace Helpers.Position
{
    public class UnmovableAfterDestroyingPoint : IUnmovableAfterDestroyingPoint, IDisposable
    {
        private IDestroyablePoint _destroyablePoint;
        private Vector2 _lastPointPosition;

        public Vector2 WorldPosition => _destroyablePoint == null ? _lastPointPosition : _destroyablePoint.WorldPosition;

        public UnmovableAfterDestroyingPoint(IDestroyablePoint destroyablePoint)
        {
            _destroyablePoint = destroyablePoint;
            _destroyablePoint.Destroying += OnPointDestroying;
        }

        public void Dispose()
        {
            if (_destroyablePoint != null)
                _destroyablePoint.Destroying -= OnPointDestroying;
        }

        public bool HasPoint(IDestroyablePoint destroyablePoint)
        {
            return _destroyablePoint == destroyablePoint;
        }

        public Vector2 Get() => WorldPosition;

        private void OnPointDestroying(IDestroyablePoint destroyablePoint)
        {
            _destroyablePoint.Destroying -= OnPointDestroying;
            _lastPointPosition = _destroyablePoint.WorldPosition;
            _destroyablePoint = null;
        }
    }
}