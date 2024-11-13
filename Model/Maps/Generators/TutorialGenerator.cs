using System.Collections.Generic;
using Model.Maps.Types;
using UnityEngine;

namespace Model.Maps.Generators
{
    public class TutorialGenerator : IMapGenerator
    {
        public IMap Generate()
        {
            return new Map(GenerateTiles());
        }

        private List<ITile> GenerateTiles()
        {
            ITile startTile = new StartTile(new Vector2Int(0, 0));
            List<ITile> tiles = new() {startTile};

            tiles.Add(new EmptyTile(new Vector2Int(1, 0)));
            tiles.Add(new EmptyTile(new Vector2Int(2, 0)));
            tiles.Add(new BossTile(new Vector2Int(3, 0)));

            return tiles;
        }
    }
}