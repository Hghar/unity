using System;
using System.Collections.Generic;

namespace Ticking
{
    public class TickablePool : ITickablePool, IGlobalTickable
    {
        private HashSet<ITickable> _tickables = new HashSet<ITickable>();
        private HashSet<ITickable> _nextTickTickables = new HashSet<ITickable>();
        private bool _isEnumerating = false;

        public bool TryAdd(ITickable tickable)
        {
            if (tickable == this)
                return false;

            return DefineActualTickables().Add(tickable);
        }

        public void PreTick(float deltaTime = 1)
        {
            throw new NotImplementedException();
        }

        public void Tick(float deltaTime = 1f)
        {
            _nextTickTickables = new HashSet<ITickable>(_tickables);

            _isEnumerating = true;
            foreach (ITickable tickable in _tickables)
            {
                tickable.PreTick(deltaTime);
            }
            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick(deltaTime);
            }
            foreach (ITickable tickable in _tickables)
            {
                tickable.LateTick(deltaTime);
            }
            _isEnumerating = false;

            _tickables = new HashSet<ITickable>(_nextTickTickables);
        }

        public void LateTick(float deltaTime = 1)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(ITickable tickable)
        {
            return DefineActualTickables().Remove(tickable);
        }

        private HashSet<ITickable> DefineActualTickables()
        {
            if (_isEnumerating)
                return _nextTickTickables;
            else
                return _tickables;
        }
    }
}