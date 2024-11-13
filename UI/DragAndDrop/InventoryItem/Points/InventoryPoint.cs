using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DragAndDrop.InventoryItem.Points
{
    public class InventoryPoint : MonoBehaviour, IConnectPoint
    {
        private readonly List<IDraggable> _attached = new List<IDraggable>();
        private RectTransform _rectTransform;
        private bool _rebuild;

        public Transform Transform => transform;
        public bool IsFree => true;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (_rebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
                _rebuild = false;
            }
        }

        public bool IsConnectable(IDraggable draggable)
        {
            if (IsFree)
                return true;

            return false;
        }


        public IConnectPoint TryConnect(IDraggable draggable)
        {
            _attached.Add(draggable);
            return this;
        }

        public IConnectPoint TryDisconnect(IDraggable draggable)
        {
            if (_attached.Contains(draggable) == false)
                return this;

            _attached.Remove(draggable);
            return this;
        }

        public void UpdateLayer()
        {
            _rebuild = true;
        }
    }
}