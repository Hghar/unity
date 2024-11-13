using UnityEngine;

namespace UI.Map
{
    public interface IMapTileViewInfo
    {
        public bool IsOpened { get; }
        public Vector2Int Position { get; }
        public bool IsCurrent { get; }
        public MapTileViewType MapTileViewType { get; }
    }
}