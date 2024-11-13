using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.InputEssence;
using UnityEngine;

namespace Infrastructure.RayCastingEssence
{
    public class RayCasting : IDisposable
    {
        private Camera _camera;
        private IInputSystem _input;

        public RayCasting(IInputSystem input)
        {
            _camera = Camera.main;
            _input = input;
        }

        public Vector3 MousePositionInWorld => (Vector2) _camera.ScreenToWorldPoint(_input.MousePosition);

        public T Cast<T>(bool closest = false) where T : Component
        {
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(_camera.ScreenPointToRay(_input.MousePosition));

            List<T> components = new List<T>();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent<T>(out T component))
                {
                    components.Add(component);
                }
            }

            if (closest)
            {
                float minDistance = Single.MaxValue;
                T closestComponent = null;
                foreach (T component in components)
                {
                    if ((component.transform.position - MousePositionInWorld).magnitude < minDistance)
                    {
                        minDistance = (component.transform.position - MousePositionInWorld).magnitude;
                        closestComponent = component;
                    }
                }

                return closestComponent;
            }

            if (components.Count > 0)
            {
                T component = components.First();

                return component;
            }

            return null;
        }

        public T[] CastAll<T>()
        {
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(_camera.ScreenPointToRay(_input.MousePosition));
            List<T> components = new List<T>();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent<T>(out T component))
                {
                    components.Add(component);
                }
            }

            return components.ToArray();
        }

        public bool Cast(ref RaycastHit2D hit)
        {
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(_camera.ScreenPointToRay(_input.MousePosition));
            if (hits.Length > 1)
            {
                hit = hits[0];
                return true;
            }

            return false;
        }

        public bool Cast(ref RaycastHit2D hit, int layers)
        {
            RaycastHit2D[] hits =
                Physics2D.GetRayIntersectionAll(_camera.ScreenPointToRay(_input.MousePosition), 30, layers);
            if (hits.Length > 1)
            {
                hit = hits[0];
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _camera = null;
            _input = null;
        }
    }
}