using System;
using UnityEngine;
using UnityEngine.Events;

namespace Realization.General
{
    public class DestroyingHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;

        public event Action Destroyed;

        private void OnDestroy()
        {
            _event?.Invoke();
            Destroyed?.Invoke();
        }
    }
}