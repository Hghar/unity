using UnityEngine;

namespace UI.Map
{
    [CreateAssetMenu(fileName = nameof(MapTileViewConfig), menuName = "Configs/" + nameof(MapTileViewConfig),
        order = 0)]
    public class MapTileViewConfig : ScriptableObject, IMapTileViewConfig
    {
        [Header("Open")] [SerializeField] private Sprite _openedBackgroundSprite;
        [SerializeField] private Color _openedBackgroundColor;
        [SerializeField] private Color _openedBordersColor;
        [Header("Close")] [SerializeField] private Sprite _closedBackgroundSprite;
        [SerializeField] private Color _closedBackgroundColor;
        [SerializeField] private Color _closedBordersColor;

        [Header("Type icons")] [SerializeField]
        private Sprite _bossIcon;

        [SerializeField] private Sprite _itemShopIcon;
        public Color GetBackgroundColor(bool isOpened)
        {
            if (isOpened)
                return _openedBackgroundColor;
            else
                return _closedBackgroundColor;
        }

        public Sprite GetBackgroundSprite(bool isOpened)
        {
            if (isOpened)
                return _openedBackgroundSprite;
            else
                return _closedBackgroundSprite;
        }

        public Color GetBordersColor(bool isOpened)
        {
            if (isOpened)
                return _openedBordersColor;
            else
                return _closedBordersColor;
        }

        public bool TryGetTypeIcon(IMapTileViewInfo mapTileViewInfo, out Sprite sprite)
        {
            switch (mapTileViewInfo.MapTileViewType)
            {
                case MapTileViewType.Boss:
                    sprite = _bossIcon;
                    return true;
                case MapTileViewType.CharacterShop:
                    sprite = _itemShopIcon;
                    return true;
                default:
                    sprite = null;
                    return false;
            }
        }
    }
}