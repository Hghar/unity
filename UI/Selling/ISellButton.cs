using System;

namespace UI.Selling
{
    public interface ISellButton
    {
        public event Action Clicked;
        void Disable();
        void Enable();
    }
}