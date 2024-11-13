using System;
using Fight.Fractions;
using UI.Selling;
using Units;
using Units.Picking;
using Zenject;

namespace UnitSelling
{
    public class PerClickSeller : IDisposable, IPerClickSeller
    {
        private ISellButton _sellButton;
        private ISeller _seller;
        private IUnitPicker _picker;
        private IUnitSellingConfig _cellConfig;

        private IUnit _unit;

        [Inject]
        private void Construct(ISeller sellable, ISellButton sellButton, IUnitPicker picker)
        {
            _picker = picker;
            _seller = sellable;
            _sellButton = sellButton;

            picker.UnitPicked += OnPicked;

            _sellButton.Clicked += OnSellButtonClicked;
        }

        private void OnPicked(IUnit unit)
        {
            _unit = unit;
            _cellConfig = unit.Parameters.CellConfig;
            SwitchButtonStatus();
        }

        private void SwitchButtonStatus()
        {
            IMinion minion = _unit as IMinion;
            if (minion.Fraction == Fraction.Enemies)
                _sellButton.Disable();
            else
                _sellButton.Enable();
        }

        public void Dispose()
        {
            _sellButton.Clicked -= OnSellButtonClicked;
        }

        private void OnSellButtonClicked()
        {
            _seller.TrySell(_cellConfig, _unit.Parameters.Level.level);
        }
    }
}