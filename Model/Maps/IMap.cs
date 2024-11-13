using System;
using System.Collections.Generic;
using Model.Maps.Types;
using UnityEngine;

namespace Model.Maps
{
    public interface IMap
    {
        IReadOnlyList<ITile> Tiles { get; }
        ITile Current { get; }

        event Action Moved;

        ITile StartTile();
        ITile MoveTo(Vector2 offset);
        Vector2[] BlockedTilesAround(ITile tile);
        Vector2 CurrentDirectionTo(ITile tile);
    }
}