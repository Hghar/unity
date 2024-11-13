using UnitSelling;
using UnitSelling.Picking;
using UnityEngine;
using Zenject;

namespace Installers.UnitSelling
{
    public class UnitSellingInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UnitSellingConfig _unitSellingConfig;

        private PickedSellable _pickedSellable;
        private PerClickSeller _perClickSeller;
        private LimitedSeller _limitedSeller;
        private Seller _seller;

        private void OnDestroy()
        {
            if(_pickedSellable != null)
                _pickedSellable.Dispose();
            if(_perClickSeller != null)
                _perClickSeller.Dispose();
            if(_limitedSeller != null)
                _limitedSeller.Dispose();
        }

        public override void InstallBindings()
        {
            Container.Bind<IUnitSellingConfig>().FromInstance(_unitSellingConfig).AsSingle();

            _pickedSellable = new PickedSellable();
            Container.Bind<IPickedSellable>().FromInstance(_pickedSellable).AsSingle();
            Container.Bind<IPickedSellablePublisher>().FromInstance(_pickedSellable).AsSingle();

            Container.Bind<ISellablePool>().To<SellablePool>().AsSingle();
            Container.Bind<ISellingPossiblityFlag>().To<SellingPossiblityFlag>().AsSingle();

            _seller = new Seller();
            _limitedSeller = new LimitedSeller(_seller);
            Container.Bind<IReadonlySeller>().FromInstance(_seller).AsSingle();
            Container.Bind<ISeller>().FromInstance(_limitedSeller).AsSingle();

            Container.BindInterfacesTo(GetType())
                            .FromInstance(this)
                            .AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(_seller);
            Container.Inject(_limitedSeller);
            _perClickSeller = Container.Instantiate<PerClickSeller>();
            Container.Inject(_pickedSellable);
        }
    }
}