using Infrastructure.RayCastingEssence;
using Model.Inventories;
using Model.Inventories.Items;
using UI.DragAndDrop.InventoryItem.Points;
using Units;
using Units.Picking;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class FollowState : ISlotState
    {
        private readonly StateArgs _args;
        private readonly RayCasting _rayCasting;
        private readonly IInventoryItem _item;
        private readonly IInventory _inventory;

        public IConnectPoint Point { get; }

        public FollowState(StateArgs args, IConnectPoint lastPoint, RayCasting rayCasting,
            IInventoryItem item, IInventory inventory)
        {
            _inventory = inventory;
            _item = item;
            _rayCasting = rayCasting;
            _args = args;
            Point = lastPoint;
            if (_args.Draggable is IItemDraggable itemDraggable)
                itemDraggable.MoveToDraggableParent();
        }

        public void Tick()
        {
            _args.Transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
        }

        public void End()
        {
            IMinion[] minions = _rayCasting.CastAll<IMinion>(); // TODO: remove this dependency

            if (minions == null)
                return;

            foreach (IMinion minion in minions)
            {
                // TODO: think how to fix problem with several colliders
                Collider2D collider = minion.GameObject.GetComponentInChildren<PickableUnit>().gameObject
                    .GetComponent<Collider2D>();
                if (collider.bounds.Contains(_rayCasting.MousePositionInWorld) == false)
                    continue;

                // if (minion.TryTakeItem(_item.GetMinionItem()))
                // {
                //     _inventory.Remove(_item);
                //     return;
                // }
            }
        }
    }
}