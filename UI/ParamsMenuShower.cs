using Realization.TutorialRealization.Helpers;
using Units;
using Units.Picking;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ParamsMenuShower : MonoBehaviour
    {
        private IUnitPicker _unitPicker;
        private IParamsMenu _paramsMenu;

        [Inject]
        private void Construct(IUnitPicker unitPicker, IParamsMenu paramsMenu)
        {
            _unitPicker = unitPicker;
            _paramsMenu = paramsMenu;
        }

        private void Awake()
        {
            if (_paramsMenu != null)
                _paramsMenu.Hide();

            _paramsMenu.Destroying += OnParamsMenuDestroying;
        }

        private void OnEnable()
        {
            _unitPicker.UnitPicked += OnUnitPicked;
            _unitPicker.UnitUnpicked += OnUnitUnpicked;
        }

        private void OnDisable()
        {
            _unitPicker.UnitPicked -= OnUnitPicked;
            _unitPicker.UnitUnpicked -= OnUnitUnpicked;
        }

        private void OnDestroy()
        {
            _paramsMenu.Destroying -= OnParamsMenuDestroying;
        }

        private void OnParamsMenuDestroying()
        {
            Destroy(this);
        }

        private void OnUnitPicked(IUnit unit)
        {
            //_paramsMenu.Bind(unit);
            //_paramsMenu.Show();
        }

        private void OnUnitUnpicked()
        {
            if (_paramsMenu.Equals(null) == false)
                _paramsMenu.Hide();
        }
    }
}