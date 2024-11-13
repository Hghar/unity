using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Ai
{
    public class MinionsAiPool : IMinionsSetPositionsPublisher, IMinionsAiPool, IDisposable
    {
        public event Action AllMinionsSetPositions;

        private readonly HashSet<IMinionAi> _minionAis = new HashSet<IMinionAi>();
        private int _minionsInPositionAmount = 0;

        public bool TryAdd(IMinionAi minionAi)
        {
            bool isAdded = _minionAis.Add(minionAi);

            if (isAdded)
                SubscribeToMinion(minionAi);

            return isAdded;
        }

        public bool TryRemove(IMinionAi minionAi)
        {
            bool isRemoved = _minionAis.Remove(minionAi);

            if (isRemoved)
                UnsubscribeFromMinion(minionAi);

            return isRemoved;
        }

        public void Dispose()
        {
            foreach (IMinionAi minionAi in _minionAis)
            {
                UnsubscribeFromMinion(minionAi);
            }

            AllMinionsSetPositions = null;
        }

        private void OnMinionTookPosition()
        {
            _minionsInPositionAmount++;
            
            if (_minionsInPositionAmount == _minionAis.Count)
            {
                AllMinionsSetPositions?.Invoke();
            }
        }

        private void OnMinionLeftPosition()
        {
            _minionsInPositionAmount--;
        }

        private void OnMinionDying(IMinionAi minionAi)
        {
            TryRemove(minionAi);
        }

        private void SubscribeToMinion(IMinionAi minionAi)
        {
            minionAi.TookPosition += OnMinionTookPosition;
            minionAi.LeftPosition += OnMinionLeftPosition;
            minionAi.Dying += OnMinionDying;
            minionAi.Destroying += OnMinionDestroying;
        }

        private void UnsubscribeFromMinion(IMinionAi minionAi)
        {
            minionAi.TookPosition -= OnMinionTookPosition;
            minionAi.LeftPosition -= OnMinionLeftPosition;
            minionAi.Dying -= OnMinionDying;
            minionAi.Destroying -= OnMinionDestroying;
        }

        private void OnMinionDestroying(IMinionAi minionAi)
        {
            TryRemove(minionAi);
        }
    }
}