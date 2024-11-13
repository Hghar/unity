using Infrastructure.InputEssence;
using Infrastructure.RayCastingEssence;
using Zenject;

namespace Realization.Installers
{
    public class RayCastingAndInputInstaller : MonoInstaller
    {
        private IInputSystem _inputSystem;
        private RayCasting _rayCasting;

        public override void InstallBindings()
        {
            _inputSystem = new DefaultInputSystem();
            _rayCasting = new RayCasting(_inputSystem);

            Container.Bind<IInputSystem>().FromInstance(_inputSystem).AsSingle();
            Container.Bind<RayCasting>().FromInstance(_rayCasting).AsSingle();
        }

        private void OnDisable()
        {
            _rayCasting.Dispose();
        }
    }
}