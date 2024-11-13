using UnityEngine;
using Zenject;
using ZoneSize;

namespace Installers
{
    public class ZoneSizeInstaller : MonoInstaller
    {
        [SerializeField] private ZoneSize.ZoneSize _zoneSize;

        private void OnValidate()
        {
            if (_zoneSize == null)
                _zoneSize = FindObjectOfType<ZoneSize.ZoneSize>();
        }

        public override void InstallBindings()
        {
            Container.Bind<IZoneSize>().FromInstance(_zoneSize).AsSingle();
        }
    }
}