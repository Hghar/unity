using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map.Mock
{
    public class MapRepresenterMock : MonoBehaviour
    {
        private readonly bool[,] TilePlacesMatrix =
        {
            {true, false, false,},
            {true, false, false},
            {true, true, true}
        };

        private readonly bool[,] IsCurrentMatrix =
        {
            {false, false, false,},
            {true, false, false},
            {false, false, false}
        };

        private readonly bool[,] IsOpenedMatrix =
        {
            {false, false, false,},
            {true, false, false},
            {true, true, false}
        };

        private readonly MapTileViewType[,] TypeMatrix =
        {
            {MapTileViewType.Boss, MapTileViewType.Empty, MapTileViewType.Empty,},
            {MapTileViewType.Empty, MapTileViewType.Empty, MapTileViewType.Empty},
            {MapTileViewType.Empty, MapTileViewType.CharacterShop, MapTileViewType.Empty}
        };

        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private MapGridFiller _gridFiller;
        [SerializeField] private FullMapSizeFitter _sizeFitter;

        private void Start()
        {
            IEnumerable<IMapTileViewInfo> tiles = CreateTiles();
            _gridFiller.FillGrid(tiles, _grid);
            _sizeFitter.FitSize(_grid);
        }

        private IEnumerable<IMapTileViewInfo> CreateTiles()
        {
            List<IMapTileViewInfo> tiles = new List<IMapTileViewInfo>();

            for (int x = 0; x < TilePlacesMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < TilePlacesMatrix.GetLength(1); y++)
                {
                    if (TilePlacesMatrix[x, y])
                    {
                        bool isOpened = IsOpenedMatrix[x, y];
                        Vector2Int position = new Vector2Int(x, y);
                        bool isCurrent = IsCurrentMatrix[x, y];
                        MapTileViewType type = TypeMatrix[x, y];

                        IMapTileViewInfo newTile = new MapTileViewInfo(isOpened, position, isCurrent, type);
                        tiles.Add(newTile);
                    }
                }
            }

            return tiles;
        }
    }
}