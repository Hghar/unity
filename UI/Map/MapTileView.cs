using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Map
{
    public class MapTileView : MonoBehaviour, IMapTileView
    {
        [SerializeField] private Image _borders;
        [SerializeField] private Image _background;
        [SerializeField] private Image _typeIcon;
        [SerializeField] private GameObject _isCurrentMarker;

        private IMapTileViewConfig _config;

        [Inject]
        private void Construct(IMapTileViewConfig config)
        {
            _config = config;
        }

        public void Represent(IMapTileViewInfo tile)
        {
            _background.sprite = _config.GetBackgroundSprite(tile.IsOpened);
            _background.color = _config.GetBackgroundColor(tile.IsOpened);
            _borders.color = _config.GetBordersColor(tile.IsOpened);

            if (_config.TryGetTypeIcon(tile, out Sprite typeIcon))
                _typeIcon.sprite = typeIcon;
            else
                _typeIcon.enabled = false;

            _isCurrentMarker.SetActive(tile.IsCurrent);
        }
    }
}