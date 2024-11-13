using System;

namespace Ticking
{
    public class TickableTimer : ITickable
    {
        private readonly float _cooldown;
        private float _lifeTime = 0f;

        public event Action Over;

        public TickableTimer(float cooldown)
        {
            _cooldown = cooldown;
        }

        public void PreTick(float deltaTime = 1)
        {
            throw new NotImplementedException();
        }

        public void Tick(float deltaTime = 1)
        {
            _lifeTime += deltaTime;

            if (_lifeTime >= _cooldown)
                Over?.Invoke();
        }

        public void LateTick(float deltaTime = 1)
        {
            throw new NotImplementedException();
        }
    }
}