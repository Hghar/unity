using System;
using System.Collections.Generic;
using System.Linq;
using Model.Maps.Types;
using UnityEngine;

namespace Model.Maps
{
    public class Map : IMap
    {
        public IReadOnlyList<ITile> Tiles { get; }
        public ITile Current { get; private set; }

        public event Action Moved;

        public Map(IReadOnlyList<ITile> tiles)
        {
            Tiles = tiles;
            Current = StartTile();
        }

        public ITile StartTile()
        {
            return Tiles.First(tile => tile is StartTile);
        }

        public ITile MoveTo(Vector2 offset)
        {
            ITile next = Next(Current, offset);
            if (next != null)
            {
                Current = next;
                Moved?.Invoke();
                return next;
            }

            return Current;
        }

        public Vector2[] BlockedTilesAround(ITile tile)
        {
            List<Vector2> blocked = new();

            if (Next(tile, Vector2.up) == null)
            {
                blocked.Add(Vector2.up);
            }

            if (Next(tile, Vector2.down) == null)
            {
                blocked.Add(Vector2.down);
            }

            if (Next(tile, Vector2.left) == null)
            {
                blocked.Add(Vector2.left);
            }

            if (Next(tile, Vector2.right) == null)
            {
                blocked.Add(Vector2.right);
            }

            return blocked.ToArray();
        }

        public Vector2 CurrentDirectionTo(ITile tile)
        {
            return new Vector2(Current.Position.x - tile.Position.x, Current.Position.y - tile.Position.y);
        }

        private ITile Next(ITile current, Vector2 offset)
        {
            return Tiles.FirstOrDefault(tile => tile.Position == current.Position + offset);
        }
    }
}