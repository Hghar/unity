using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.RayCastingEssence;
using Model.Inventories;
using Model.Inventories.Items;
using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class DraggableStateMachine
    {
        private const float MinDistance = 10;
        private const float MinAngle = 50;
        private const float MinDistanceToNotMove = 2;

        private StateArgs _args;
        private ISlotState _currentState;
        private Vector3 _tapPos;
        private bool _isNotReadyToMove;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;
        private RayCasting _rayCasting;
        private IInventoryItem _item;
        private IInventory _inventory;

        public DraggableStateMachine(StateArgs args, RayCasting rayCasting,
            IInventoryItem item, IInventory inventory)
        {
            try
            {
                _inventory = inventory;
                _item = item;
                _rayCasting = rayCasting;
                UpdateArgs(args);
                _currentState = new InventoryState(_args);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void UpdateItem(IInventoryItem item)
        {
            _item = item;
        }

        private void UpdateArgs(StateArgs args)
        {
            _args = args;
        }

        public void Tick()
        {
            _currentState.Tick();
        }

        public void ChangeState(DraggableStates newState)
        {
            _currentState.End();
            switch (newState)
            {
                case DraggableStates.Downed:
                    ChangeToDownedState();
                    break;
                case DraggableStates.Upped:
                    _tokenSource.Cancel();
                    SelectStateOnUpped();
                    break;
                case DraggableStates.MovingToInventory:
                    _currentState = new InventoryState(_args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void ChangeToDownedState()
        {
            _currentState = new ClickedState(_currentState.Point);
            _tapPos = Input.mousePosition;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartClickAwaiting(_token);
        }

        private void SelectStateOnUpped()
        {
            if (_isNotReadyToMove)
            {
                if (_currentState.Point is InventoryPoint)
                    _currentState = new InventoryState(_args);
                else
                    _currentState = new BuildState(_args);

                _isNotReadyToMove = false;
                return;
            }

            if (_currentState is FollowState || _isNotReadyToMove)
            {
                _currentState = new IdleState(_args);
            }
            else
            {
                if (_currentState.Point is InventoryPoint)
                    _currentState = new BuildState(_args);
                else
                    _currentState = new InventoryState(_args);
            }
        }

        private async void StartClickAwaiting(CancellationToken cancellationToken)
        {
            Vector3 newPos;
            float endTime = 0;
            float angle;
            float distance;
            _isNotReadyToMove = false;

            while (endTime < _args.TimeToDrag)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                newPos = Input.mousePosition;
                angle = GetAngle(_tapPos, newPos);
                distance = ((Vector2) _tapPos - (Vector2) newPos).magnitude;

                if (distance > MinDistance && angle < MinAngle)
                {
                    _currentState.End();
                    _currentState = new FollowState(_args, _currentState.Point, _rayCasting, _item, _inventory);
                    // TODO: remove item and inventory dependency
                    _isNotReadyToMove = false;
                    return;
                }

                if (distance > MinDistanceToNotMove)
                    _isNotReadyToMove = true;

                endTime += Time.deltaTime;
                await Task.Yield();
            }

            _isNotReadyToMove = true;
        }

        private float GetAngle(Vector3 oldPos, Vector3 newPos)
        {
            float angle = Vector3.Angle(Vector3.right, (oldPos - newPos).normalized);
            if (_currentState.Point is BuildPoint)
                angle = 0;

            return angle;
        }
    }
}