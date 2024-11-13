using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.TickInvokerService
{
    internal class EventAction : IDisposable
    {
        public Action Action;
        public bool IsDisposed { get; private set; }

        public EventAction(Action action)
        {
            Action = action;
        }

        public void Dispose()
        {
            Action = null;
            IsDisposed = true;
        }
    }
    internal class EventActionList
    {
        private readonly List<EventAction> _actionsList = new();

        public void InvokeActions()
        {
            for (var i = 0; i < _actionsList.Count; i++)
            {
                EventAction eventAction = _actionsList[i];
                if (!eventAction.IsDisposed)
                    eventAction.Action.Invoke();
            }
        }

        internal void RemoveActions()
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventAction eventAction = _actionsList[i];
                if (eventAction.IsDisposed)
                {
                    _actionsList.RemoveAt(i);
                }
            }
        }

        internal void Add(Action action)
        {
            EventAction newAction = new EventAction(action);
            _actionsList.Add(newAction);
        }

        internal void Remove(Action action)
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventAction eventAction = _actionsList[i];
                if (eventAction.Action == action)
                    eventAction.Dispose();
            }
        }

        internal void Clear()
        {
            _actionsList.Clear();
        }
    }
    public enum UpdateType
    {
        Update = 0,
        FixedUpdate = 1,
        LateUpdate = 2
    }
    public class TickInvoker : ITickable, ILateTickable, IFixedTickable
    {
        private bool _isPause;
        public float DeltaTime { get; private set; }
        public float FixedDeltaTime { get; private set; }
        
        private readonly EventActionList _onUpdateActions = new();
        private readonly EventActionList _onLateUpdateActions = new();
        private readonly EventActionList _onFixedUpdateActions = new();

        public void Tick()
        {
            if (_isPause)
                return;

            DeltaTime = Time.deltaTime;
            _onUpdateActions.InvokeActions();
        }

        public void LateTick()
        {
            if (_isPause)
                return;

            _onLateUpdateActions.InvokeActions();
        }

        public void SetPause(bool isPause)
        {
            _isPause = isPause;
        }

        public void FixedTick()
        {
            if (_isPause)
                return;

            FixedDeltaTime = Time.fixedDeltaTime;
            _onFixedUpdateActions.InvokeActions();
        }

        public IDisposable Subscribe(UpdateType eventType, Action action)
        {
            switch (eventType)
            {
                case UpdateType.Update:
                    _onUpdateActions.Add(action);
                    break;
                case UpdateType.FixedUpdate:
                    _onFixedUpdateActions.Add(action);
                    break;
                case UpdateType.LateUpdate:
                    _onLateUpdateActions.Add(action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }

            return new DisposableActionHolder(() =>
            {
                Unsubscribe(eventType, action);
            });
        }

        public void Unsubscribe(UpdateType eventTypeType, Action action)
        {
            switch (eventTypeType)
            {
                case UpdateType.Update:
                    _onUpdateActions.Remove(action);
                    break;
                case UpdateType.FixedUpdate:
                    _onFixedUpdateActions.Remove(action);
                    break;
                case UpdateType.LateUpdate:
                    _onLateUpdateActions.Remove(action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventTypeType), eventTypeType, null);
            }
        }
        
        public sealed class DisposableActionHolder : IDisposable
        {
            private readonly Action _disposeAction;
            public DisposableActionHolder(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }
            public void Dispose()
            {
                _disposeAction?.Invoke();
            }
        }
    }
}