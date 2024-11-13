using System;
using Units;
using UnityEngine;

namespace Model.Inventories.Items
{
    public interface IInventoryItem : IDisposable
    {
        event Action Disposed;

        string Name { get; }
        Sprite Sprite { get; }

        IMinionItem GetMinionItem();
    }
}