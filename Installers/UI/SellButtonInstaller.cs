using UI.Selling;
using Units;
using UnityEngine;
using Zenject;

namespace Installers.UI
{
    public class SellButtonInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private SellButton _sellButton;

        private SellButtonSwitcher _sellButtonSwitcher;
        private SellButtonInteractivitySwitcher _sellButtonInteractivitySwitcher;

        private void OnValidate()
        {
            if (_sellButton == null)
                _sellButton = FindObjectOfType<SellButton>();
        }

        private void OnDestroy()
        {
            if(_sellButtonSwitcher != null)
                _sellButtonSwitcher.Dispose();
            if(_sellButtonInteractivitySwitcher != null)
                _sellButtonInteractivitySwitcher.Dispose();
        }

        public override void InstallBindings()
        {
            Container.Bind<ISellButton>().FromInstance(_sellButton).AsSingle();
            Container.Bind<ISwitchableSellButton>().FromInstance(_sellButton).AsSingle();
            Container.Bind<IInteractivitySwitchableSellButton>().FromInstance(_sellButton).AsSingle();
            Container.BindInterfacesTo<SellButtonInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize()
        {
            _sellButtonSwitcher = Container.Instantiate<SellButtonSwitcher>();
            _sellButtonInteractivitySwitcher = Container.Instantiate<SellButtonInteractivitySwitcher>();
        }
    }
}