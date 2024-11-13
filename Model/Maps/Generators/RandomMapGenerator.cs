using System.Collections.Generic;
using Infrastructure.Shared.Extensions;
using Model.Maps.Types;
using UnityEngine;

namespace Model.Maps.Generators
{
    public class RandomMapGenerator : IMapGenerator
    {
        private readonly float _size;

        public RandomMapGenerator(float size)
        {
            _size = size;
        }

        public IMap Generate()
        {
            ITile startTile = new StartTile(new Vector2Int(0, 0));
            List<ITile> tiles = new()
            {
                startTile
            };

            tiles.AddRange(GenerateEmptyTiles(startTile.Position));

            return new Map(tiles);
        }

        private List<ITile> GenerateEmptyTiles(Vector2Int startPosition)
        {
            List<ITile> emptyTiles = new();
            List<Vector2Int> positions = new()
            {
                startPosition
            };

            for (int i = 0; i < _size; i++)
            {
                AddEmptyTileTo(positions);
            }

            positions.RemoveAt(0);

            foreach (Vector2Int position in positions)
            {
                emptyTiles.Add(new EmptyTile(position));
            }

            return emptyTiles;
        }

        private void AddEmptyTileTo(List<Vector2Int> positions, List<Vector2Int> offsets = null)
        {
            List<Vector2Int> shuffledPositions = new(positions);
            shuffledPositions.Shuffle();

            offsets ??= new()
            {
                new Vector2Int(1, 0),
                //new Vector2Int(0, 1),
                //new Vector2Int(-1, 0),
                //new Vector2Int(0, -1)
            };

            foreach (Vector2Int position in shuffledPositions)
            {
                offsets.Shuffle();

                foreach (Vector2Int offset in offsets)
                {
                    if (FreePosition(positions, position + offset))
                    {
                        positions.Add(position + offset);
                        return;
                    }
                }
            }
        }

        private bool FreePosition(List<Vector2Int> usedPositions, Vector2Int position)
        {
            if (usedPositions.Exists(pos => pos == position))
            {
                return false;
            }

            return true;
        }
    }
}