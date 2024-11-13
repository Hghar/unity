using System;
using Units;

namespace UI
{
    public interface IParamsMenu
    {
        public event Action Destroying;

        public void Hide();
        public void Show();
        public void Bind(IUnit unit);
    }
}