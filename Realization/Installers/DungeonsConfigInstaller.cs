using Realization.States.CharacterSheet;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class DungeonsConfigInstaller : MonoInstaller
    {
        [SerializeField] private DungeonConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<DungeonConfig>().FromInstance(_config).AsSingle();
        }
    }
}