using UnityEngine;

namespace UI.Map
{
    public class MapTileViewInfo : IMapTileViewInfo
    {
        private readonly bool _isOpened;
        private readonly Vector2Int _position;
        private readonly bool _isCurrent;
        private readonly MapTileViewType _mapTileViewType;

        public bool IsOpened => _isOpened;
        public Vector2Int Position => _position;
        public bool IsCurrent => _isCurrent;
        public MapTileViewType MapTileViewType => _mapTileViewType;

        public MapTileViewInfo(bool isOpened, Vector2Int position, bool isCurrent, MapTileViewType mapTileViewType)
        {
            _isOpened = isOpened;
            _position = position;
            _isCurrent = isCurrent;
            _mapTileViewType = mapTileViewType;
        }
    }
}