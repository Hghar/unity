using Parameters;
using Realization.Configs;
using Realization.States.CharacterSheet;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CharacterConfigInstaller : MonoInstaller
    {
        [SerializeField] private CharacterConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<CharacterConfig>().FromInstance(_config).AsSingle();

            Container.Bind<CharacterSet[]>().FromInstance(_config.CharacterSets.ToArray()).AsSingle();
            Container.Bind<Constants>().FromInstance(_config.Constants).AsSingle();
        }
    }
}
