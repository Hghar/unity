using System.Collections.Generic;
using System.Linq;
using Infrastructure.Shared.Extensions;
using Model.Maps.Types;
using UnityEngine;

namespace Model.Maps.Generators
{
    public class LineMapGenerator : IMapGenerator
    {
        private static readonly Vector2Int[] Offsets =
        {
            new(1, 0),
            //new(0, 1),
            //new(-1, 0),
            //new(0, -1)
        };

        private readonly float _size;
        private readonly int _segmentLength;
        private readonly int _shopCount;
        private readonly bool _spawnShopsOnEmpty;
        private readonly float _rotateChance;
        private readonly float _roomChangeChance;

        public LineMapGenerator(
            float size,
            int segmentLength,
            int shopCount,
            bool spawnShopsOnEmpty,
            float rotateChance,
            float roomChangeChance)
        {
            _size = size;
            _segmentLength = segmentLength;
            _shopCount = shopCount;
            _spawnShopsOnEmpty = spawnShopsOnEmpty;
            _rotateChance = rotateChance;
            _roomChangeChance = roomChangeChance;
        }


        public IMap Generate()
        {
            return new Map(GenerateTiles());
        }

        private List<ITile> GenerateTiles()
        {
            ITile startTile = new StartTile(new Vector2Int(0, 0));
            List<ITile> tiles = new() {startTile};

            AddMainWay(tiles);
            //AddShops(tiles);
            AddBossTile(tiles);

            return tiles;
        }

        private void AddMainWay(List<ITile> tiles)
        {
            List<Vector2Int> positions = GetPositionsFrom(tiles);
            int offsetIndex = Random.Range(0, Offsets.Length);

            for (int i = 0; i < _size; i++)
            {
                if (i % _segmentLength == 0)
                {
                    offsetIndex = ChangeOffsetIndex(offsetIndex, Offsets.ToList());
                }

                Vector2Int nextPosition = positions.Last() + Offsets[offsetIndex];

                if (IsFreePosition(positions, nextPosition))
                {
                    tiles.Add(new EmptyTile(nextPosition));
                    positions.Add(nextPosition);
                }
                else
                {
                    // TODO: check if looping
                    Vector2Int position = FindFreePositionsAround(positions, Offsets)
                        .ToList()
                        .Random();

                    tiles.Add(new EmptyTile(position));
                    positions.Add(position);
                }
            }
        }

        private void AddBossTile(List<ITile> tiles)
        {
            List<Vector2Int> positions = GetPositionsFrom(tiles);
            List<Vector2Int> offsets = new List<Vector2Int>(Offsets);

            Vector2Int position = FindFreePositionsAround(positions, offsets.Shuffle())
                .OrderBy(pos => Mathf.Abs(pos.x) + Mathf.Abs(pos.y))
                .ToList()
                .Last();

            tiles.Add(new BossTile(position));
        }

        private bool AddShops(List<ITile> tiles)
        {
            List<Vector2Int> offsets = new List<Vector2Int>(Offsets);
            offsets.Shuffle();

            List<KeyValuePair<ITile, int>> cache = new List<KeyValuePair<ITile, int>>();

            for (int i = 0; i < _shopCount; i++)
            {
                Vector2Int position;

                if (_spawnShopsOnEmpty && Random.Range(0f, 1f) <= _roomChangeChance &&
                    tiles.Any(tile => tile is EmptyTile))
                {
                    ITile randomEmptyTile = tiles.FindAll(tile => tile is EmptyTile).Random();
                    int index = tiles.IndexOf(randomEmptyTile);
                    position = randomEmptyTile.Position;
                    cache.Add(new KeyValuePair<ITile, int>(new ShopTile(position), index));
                    continue;
                }

                if (TryFindFreePositionNearEmpty(tiles, offsets, out position))
                {
                    tiles.Add(new ShopTile(position));
                }
                else
                {
                    Debug.LogWarning("All shops were not located. Some ones were missed");
                    return false;
                }
            }

            foreach (KeyValuePair<ITile, int> pair in cache)
            {
                tiles[pair.Value] = pair.Key;
            }

            return true;
        }

        private bool TryFindFreePositionNearEmpty(List<ITile> tiles, List<Vector2Int> offsets, out Vector2Int position)
        {
            List<ITile> emptyTiles = tiles.Where(tile => tile is EmptyTile).ToList();
            List<Vector2Int> allFreePositions = FindFreePositionsAround(GetPositionsFrom(tiles), offsets);
            List<Vector2Int> freePositionsAroundEmptyTiles =
                FindFreePositionsAround(GetPositionsFrom(emptyTiles), offsets);

            List<Vector2Int> positions = allFreePositions
                .Intersect(freePositionsAroundEmptyTiles)
                .ToList();

            if (positions.Count > 0)
            {
                position = positions.Random();
                return true;
            }

            if (freePositionsAroundEmptyTiles.Count > 0)
            {
                position = freePositionsAroundEmptyTiles.Random();
                return true;
            }

            if (allFreePositions.Count > 0)
            {
                position = allFreePositions.Random();
                return true;
            }

            position = Vector2Int.zero;
            return false;
        }

        private List<Vector2Int> GetPositionsFrom(List<ITile> tiles)
        {
            return tiles.Select(p => p.Position).ToList();
        }

        private int ChangeOffsetIndex(int index, List<Vector2Int> offsets)
        {
            if (Random.Range(0f, 1f) < _rotateChance)
            {
                int changeType = Random.Range(0, 2);
                switch (changeType)
                {
                    case 0:
                        index -= 1;
                        break;
                    case 1:
                        index += 1;
                        break;
                }
            }

            index %= offsets.Count;
            if (index == -1)
                index = offsets.Count - 1;

            return index;
        }

        private List<Vector2Int> FindFreePositionsAround(List<Vector2Int> positions, IList<Vector2Int> offsets = null)
        {
            List<Vector2Int> free = new();

            offsets ??= new List<Vector2Int>(Offsets);
            offsets.Shuffle();

            foreach (Vector2Int position in positions)
            {
                foreach (Vector2Int offset in offsets)
                {
                    if (IsFreePosition(positions, position + offset))
                    {
                        free.Add(position + offset);
                    }
                }
            }

            return free;
        }

        private bool IsFreePosition(List<Vector2Int> usedPositions, Vector2Int position)
        {
            if (usedPositions.Exists(pos => pos == position))
                return false;

            return true;
        }
    }
}