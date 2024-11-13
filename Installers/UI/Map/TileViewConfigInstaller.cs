using UI.Map;
using UnityEngine;
using Zenject;

namespace Installers.UI.Map
{
    public class TileViewConfigInstaller : MonoInstaller
    {
        [SerializeField] private MapTileViewConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<IMapTileViewConfig>().FromInstance(_config).AsSingle();
        }
    }
}