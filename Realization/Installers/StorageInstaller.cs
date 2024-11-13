using Entities.Resources;
using Infrastructure.CompositeDirector;
using Model.Economy;
using Model.Economy.Resources;
using Realization.Economy;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class StorageInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private EconomyBehaviour _behaviour;

        private IResource _gold;
        private IStorage _storage;
        private CompositeDirector _director;
        private ResourceFactory _resourceFactory;

        [Inject]
        private void Construct(CompositeDirector director)
        {
            _director = director;
        }

        public override void InstallBindings()
        {
            /*_resourceFactory = new ResourceFactory();
            ResourceEntityFactory resourceEntityFactory = new ResourceEntityFactory(_director);

            int index = PlayerPrefs.GetInt("level");
            if (PlayerPrefs.GetInt("level") == 1)
            {
                //PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("level", 1);
            }

            _gold = _resourceFactory.Create(Currency.Gold);
            if (index == 0 && _gold.Value == 0)
            {
                _gold.Add(300);
            }
            else
            {
                _gold.TrySubtract(_gold.Value);
                _gold.Add(300);
            }

            IResource[] resources = {_gold};
            _storage = new Storage(resources);*/
            //ResourceEventPublisherPool resourceEventPublisherPool = new ResourceEventPublisherPool(resources);

            //Container.Bind<ResourceEntityFactory>().FromInstance(resourceEntityFactory).AsSingle();
            //Container.Bind<IStorage>().To<Storage>().AsSingle();
            //Container.Bind<ResourceEventPublisherPool>().FromInstance(resourceEventPublisherPool).AsSingle();
            //Container.BindInterfacesTo<StorageInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize()
        {
           // _behaviour.Initialize(); //ToDo Move economyBehavior to Hud
        }
    }
}