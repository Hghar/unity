using Ticking;
using Zenject;

namespace Installers.Ticking
{
    public class TickingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var tickablePool = new TickablePool();
            Container.Bind<ITickablePool>().FromInstance(tickablePool).AsSingle();
            Container.Bind<IGlobalTickable>().FromInstance(tickablePool).AsSingle();
        }
    }
}