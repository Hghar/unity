using Model.Inventories.Items;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.Points
{
    public class BuildPoint : MonoBehaviour, IConnectPoint, IItemProvider
    {
        private IItemDraggable _attached;
        private IInventoryItem _assignedItem;

        public Transform Transform => transform;

        public bool IsFree { get; private set; }
        public IInventoryItem Item => _assignedItem;

        private void Awake()
        {
            IsFree = true;
        }

        private void OnEnable()
        {
            if (_attached != null)
            {
                _attached.Upped -= OnDropping;
                _attached.Upped += OnDropping;
            }
        }

        private void OnDisable()
        {
            if (_attached != null)
                _attached.Upped -= OnDropping;
        }

        public bool IsConnectable(IDraggable draggable)
        {
            if (draggable is IItemDraggable itemDraggable &&
                (IsFree || draggable != _attached) &&
                gameObject.activeInHierarchy)
                return true;

            return false;
        }

        public IConnectPoint TryConnect(IDraggable draggable)
        {
            IItemDraggable itemDraggable = draggable as IItemDraggable;
            if (IsConnectable(draggable) == false)
                return this;

            if (_attached != null)
                ReturnAttached(_attached);

            _attached = itemDraggable;
            //attach 
            IsFree = false;
            return this;
        }

        private void ReturnAttached(IItemDraggable attached)
        {
            TryDisconnect(attached);
            attached.ReturnToInventory();
        }

        private void OnDropping()
        {
            //TryDisconnect(_attached);
        }

        public IConnectPoint TryDisconnect(IDraggable draggable)
        {
            if (draggable != _attached)
                return this;

            _attached.Upped -= OnDropping;
            _attached = null;
            _assignedItem = null;
            IsFree = true;
            return this;
        }

        public void DisconnectCurrent()
        {
            _attached.Upped -= OnDropping;
            _attached = null;
            _assignedItem = null;
            IsFree = true;
        }
    }
}