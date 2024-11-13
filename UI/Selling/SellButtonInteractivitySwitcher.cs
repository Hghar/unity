using System;
using UnitSelling;
using Zenject;

namespace UI.Selling
{
    public class SellButtonInteractivitySwitcher : IDisposable
    {
        private ISellingPossiblityFlag _canSellFlag;
        private IInteractivitySwitchableSellButton _sellButton;

        [Inject]
        private void Construct(ISellingPossiblityFlag canSellFlag, IInteractivitySwitchableSellButton sellButton)
        {
            _canSellFlag = canSellFlag;
            _sellButton = sellButton;
            _canSellFlag.ValueChanged += OnSellingOpportunityChanged;
        }

        public void Dispose()
        {
            _canSellFlag.ValueChanged -= OnSellingOpportunityChanged;
        }

        private void OnSellingOpportunityChanged()
        {
            if(_sellButton == null || _sellButton.Equals(null))
                return;
            
            if (_canSellFlag.Value)
                _sellButton.SwitchInteractivityOn();
            else
                _sellButton.SwitchInteractivityOff();
        }
    }
}