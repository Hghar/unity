using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Realization.DevTools
{
    public class PointerHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        private void OnDestroy()
        {
            Clicked = null;
        }
    }
}