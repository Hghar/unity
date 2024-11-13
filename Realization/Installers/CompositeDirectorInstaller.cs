using System;
using CompositeDirectorWithGeneratingComposites.CompositeDirector;
using Infrastructure.CompositeDirector.Composites;
using Model.Commands;
using Model.Composites;
using Model.Composites.Hidable;
using Model.Composites.Representation;
using Model.Composites.Savable;
using Zenject;
using CompositeDirector = Infrastructure.CompositeDirector.CompositeDirector;
using NewDirector = Plugins.CompositeDirectorPlugin.CompositeDirector;

namespace Realization.Installers
{
    public class CompositeDirectorInstaller : MonoInstaller
    {
        private CompositeDirector _director;
        private NewDirector _newDirector;

        public override void InstallBindings()
        {
            //Old
            _director = new CompositeDirector(AllComposites.Composites);

            Container.Bind<CompositeDirector>().FromInstance(_director).AsSingle();

            Composite<IRepresentation> representation = new Composite<IRepresentation>(_director);
            Composite<ISavable> savable = new Composite<ISavable>(_director);
            Composite<IHidable> hidable = new Composite<IHidable>(_director);

            Container.Bind<Composite<IRepresentation>>().FromInstance(representation).AsSingle();
            Container.Bind<Composite<ISavable>>().FromInstance(savable).AsSingle();
            Container.Bind<Composite<IHidable>>().FromInstance(hidable).AsSingle();
            
            //New
            _newDirector = new NewDirector();
            Container.Bind<NewDirector>().FromInstance(_newDirector).AsSingle();

            var affectable = _newDirector.SetupComposite<IAffectable>();
            Container.Bind<IAffectable>().FromInstance(affectable as IAffectable).AsSingle();
        }

        private void LateUpdate()
        {
            lock (CompositeHelper.Stash)
            {
                CompositeHelper.Perform();
            }
        }
    }
}