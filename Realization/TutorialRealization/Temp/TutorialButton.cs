using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Realization.TutorialRealization.Temp
{
    public class TutorialButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}