using System;

namespace Units.Ai
{
    public interface IMinionAi
    {
        public event Action TookPosition;
        public event Action LeftPosition;
        public event Action<IMinionAi> Dying;
        public event Action<IMinionAi> Destroying;
    }
}