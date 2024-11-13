using Realization.General;
using Zenject;

namespace Realization.Installers
{
    public class RestarterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IRestarter>().To<Restarter>().AsSingle().NonLazy();
        }

        public void OnDestroy()
        {
            ((Restarter) Container.Resolve<IRestarter>()).Dispose();
        }
    }
}