using System;

namespace Battle
{
    public interface IStartBattleButton
    {
        public event Action Clicked;

        public void SetActive(bool value);
    }
}