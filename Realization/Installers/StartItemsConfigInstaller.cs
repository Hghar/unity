using Realization.Configs;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class StartItemsConfigInstaller : MonoInstaller
    {
        [SerializeField] private StartItemsConfig _startItemsConfig;

        public override void InstallBindings()
        {
            Container.Bind<StartItemsConfig>().FromInstance(_startItemsConfig).AsSingle();
        }
    }
}