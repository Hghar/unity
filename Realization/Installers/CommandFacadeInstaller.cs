using System;
using Model.Commands;
using Model.Commands.Creation;
using Model.Commands.Helpers;
using Realization.Sets;
using Realization.VisualEffects;
using Zenject;

namespace Realization.Installers
{
    public class CommandFacadeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IVisualEffectService>().FromInstance(new VisualEffectsService(Container)).AsSingle();
            
            Container.Bind<CommandWorker>().To<CommandWorker>().AsSingle();
            Container.Bind<CommandDeactivator>().To<CommandDeactivator>().AsSingle();
            Container.Bind<CommandBuilder>().To<CommandBuilder>().AsSingle();
            Container.Bind<CommandFacade>().To<CommandFacade>().AsSingle();
            Container.Bind<SetEffectInvoker>().To<SetEffectInvoker>().AsSingle();
        }

        private void Awake()
        {
            Container.Resolve<SetEffectInvoker>().Init();
        }

        private void OnDestroy()
        {
            Container.Resolve<CommandFacade>().Dispose();
            Container.Resolve<SetEffectInvoker>().Dispose();
        }
    }
}