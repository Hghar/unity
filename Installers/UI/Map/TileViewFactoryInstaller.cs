using UI.Map;
using UnityEngine;
using Zenject;

namespace Installers.UI.Map
{
    public class TileViewFactoryInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private MapTileView _prefab;

        public override void InstallBindings()
        {
            Container.Bind<IMapTileViewFactory>()
                .To<MapTileViewFactory>()
                .AsSingle();

            Container.BindInterfacesTo(GetType())
                .FromInstance(this)
                .AsSingle();
        }

        public void Initialize()
        {
            MapTileViewFactory factory = (MapTileViewFactory) Container.Resolve<IMapTileViewFactory>();
            factory.Load(_prefab);
        }
    }
}