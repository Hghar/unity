using UnityEngine;
using Zenject;

namespace UI.Map
{
    public class MapTileViewFactory : IMapTileViewFactory
    {
        private MapTileView _prefab;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Load(MapTileView prefab)
        {
            _prefab = prefab;
        }

        public void Create(Transform parent, IMapTileViewInfo info)
        {
            MapTileView newTile = _diContainer.InstantiatePrefabForComponent<MapTileView>(_prefab, parent);
            newTile.Represent(info);
        }
    }
}