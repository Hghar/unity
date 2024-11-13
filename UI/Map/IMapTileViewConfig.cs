using UnityEngine;

namespace UI.Map
{
    public interface IMapTileViewConfig
    {
        public Sprite GetBackgroundSprite(bool isOpened);
        public bool TryGetTypeIcon(IMapTileViewInfo tile, out Sprite sprite);
        public Color GetBackgroundColor(bool isOpened);
        public Color GetBordersColor(bool isOpened);
    }
}