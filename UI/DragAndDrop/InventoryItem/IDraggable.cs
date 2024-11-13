using System;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem
{
    public interface IDraggable
    {
        event Action Upped;

        Vector3 Position { get; }
    }
}