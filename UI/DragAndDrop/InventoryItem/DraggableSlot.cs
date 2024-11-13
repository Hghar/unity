using System;
using System.Collections.Generic;
using System.Linq;
using AssetStore.HeroEditor.FantasyInventory.Scripts.Enums;
using Infrastructure.RayCastingEssence;
using Model.Inventories;
using Model.Inventories.Items;
using UI.DragAndDrop.InventoryItem.Points;
using UI.DragAndDrop.InventoryItem.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.DragAndDrop.InventoryItem
{
    public class DraggableSlot : MonoBehaviour, IPointerDownHandler, IItemDraggable
    {
        [SerializeField] private float _timeToDrag;

        private DraggableStateMachine _stateMachine;
        private ScrollRect _scrollbar;

        private DraggingObjectParent _draggingObjectParent;
        private IInventoryItem _item;
        private bool _pointerDowned;
        private ItemType _type;
        private IConnectPointList _connectPointList;
        private RayCasting _rayCasting;
        private IInventory _inventory;

        public Vector3 Position => transform.position;
        public ItemType Type => _type;

        public IInventoryItem Item
        {
            get => _item;
            set
            {
                _stateMachine?.UpdateItem(value);
                _item = value;
            }
        }

        public event Action Upped;

        [Inject]
        private void Construct(IConnectPointList connectPointList, DraggingObjectParent draggingObjectParent,
            RayCasting rayCasting,
            IInventory inventory)
        {
            _inventory = inventory;
            _rayCasting = rayCasting;
            _connectPointList = connectPointList;
            _draggingObjectParent = draggingObjectParent;
        }

        private void Awake()
        {
            _scrollbar = GameObject.Find("Inventory_ScrollView").GetComponent<ScrollRect>();
            _stateMachine = new DraggableStateMachine(new StateArgs(transform, _timeToDrag, _connectPointList, this),
                _rayCasting, _item, _inventory);
        }

        private void Update()
        {
            _stateMachine.Tick();
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count(result => result.gameObject == gameObject) == 1 && Input.GetMouseButtonUp(0) &&
                _pointerDowned == false)
            {
                _stateMachine.ChangeState(DraggableStates.Downed);
                _pointerDowned = true;
                Upped?.Invoke();
            }

            if (Input.GetMouseButtonUp(0) && _pointerDowned)
            {
                _stateMachine.ChangeState(DraggableStates.Upped);
                _pointerDowned = false;
                _scrollbar.enabled = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _stateMachine.ChangeState(DraggableStates.Downed);
            _pointerDowned = true;
            Upped?.Invoke();
        }

        public void Init(IInventoryItem item, ItemType type)
        {
            _item = item;
            _type = type;
        }

        public void ReturnToInventory()
        {
            _stateMachine.ChangeState(DraggableStates.MovingToInventory);
        }

        public void MoveToDraggableParent()
        {
            transform.SetParent(_draggingObjectParent.transform);
            _scrollbar.enabled = false;
        }
    }
}