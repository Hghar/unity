using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private Camera _camera;

        public event Action DraggingBegun;
        public event Action DraggingFinished;
        public bool Working = true;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnDestroy()
        {
            DraggingBegun = null;
            DraggingFinished = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(Working == false)
                return;
            DraggingBegun?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(Working == false)
                return;
            Vector3 onScreenPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(onScreenPosition.x, onScreenPosition.y, transform.position.z);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Working == false)
                return;
            DraggingFinished?.Invoke();
        }
    }
}