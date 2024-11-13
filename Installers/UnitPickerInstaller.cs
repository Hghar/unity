using System;
using Realization.TutorialRealization.Helpers;
using UI;
using UI.Selling;
using Units;
using Units.Picking;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UnitPickerInstaller : MonoInstaller
    {
        [SerializeField] private PickableUnitPool _pickableUnitPool;

        public IUnitPicker _picker;
        private IParamsMenu _paramsMenu;
        private InfoHint _infoHint;

        [Inject]
        private void Construct(IParamsMenu paramsMenu, InfoHint infoHint) 
        {
            _paramsMenu = paramsMenu;
            _infoHint = infoHint;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<IPickableUnitPool>().FromInstance(_pickableUnitPool).AsSingle();
            Container.Bind<IUnitPicker>().To<UnitPicker>().AsSingle();
        }

        private void Awake()
        {
            _picker = Container.Resolve<IUnitPicker>();
            _picker.UnitPicked += unit =>
            {
                _paramsMenu.Bind(unit);
                _paramsMenu.Show();
                _infoHint.ShowOnMinion(unit);
            };
            _picker.UnitUnpicked += () =>
            {
                if(_paramsMenu != null &&
                   _paramsMenu.Equals(null) == false &&
                   HardTutorial.Activated == false)
                    _paramsMenu.Hide();
            };
        }

        private void OnValidate()
        {
            if (_pickableUnitPool == null)
                _pickableUnitPool = FindObjectOfType<PickableUnitPool>();
        }

        private void OnDestroy()
        {
            IUnitPicker unitPicker = Container.Resolve<IUnitPicker>();
            if (unitPicker != null)
            {
                ((UnitPicker) unitPicker).Dispose();
            }
        }
    }
}