using System;
using Units;
using UnityEngine;

namespace Model.Shops
{
    public interface IShopItem
    {
        event Action Bought;

        string Name { get; }
        int Price { get; }
        Sprite Sprite { get; }
        int Health { get; }
        MinionClass Class { get; }
        int Might { get; }
        int Power { get; }
        int AttackSpeed { get; }
        int Armor { get; }

        void TryBuy();
    }
}