using Entities.Tile.Views;
using Realization.Utils;
using UnityEngine;

namespace Realization.Configs
{
    [CreateAssetMenu(fileName = "New Map Config", menuName = "Configs/Map Config", order = 0)]
    public class MapConfig : ScriptableObject
    {
        [field: SerializeField] public int MapSize { get; private set; }
        [field: SerializeField] public int ShopCount { get; private set; }
        [field: SerializeField] public int MinSegmentLength { get; private set; }
        [field: SerializeField] public bool SpawnShopsOnEmpty { get; private set; }
        [field: SerializeField] public float RotateChance { get; private set; }
        [field: SerializeField] public float ChangeRoomChance { get; private set; }

        // TODO: move to room config if its needed
        [SerializeField] private Vector2 _roomSize;

        public Vector2 RoomSize
        {
            get
            {
                float aspect = ((float) Screen.width) / Screen.height;
                float modificator = aspect / ScreenUtils.DefaultAspect;

                return _roomSize * new Vector2(modificator, 1);
            }
        }

        [field: SerializeField] public PlayableTileView EmptyTileViewPrefab { get; private set; }
        [SerializeField] public PlayableTileView[] _shopTileViewPrefabs => new[] {EmptyTileViewPrefab};
        private int _currentShopIndex;
        public PlayableTileView[] ShopTileViewPrefabs => _shopTileViewPrefabs;

        [field: SerializeField] public PlayableTileView StartTileViewPrefab { get; private set; }
        [field: SerializeField] public PlayableTileView BossTileViewPrefab { get; private set; }
    }
}