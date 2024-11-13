using System;
using Battle;
using Realization.TutorialRealization.Helpers;
using Units;
using UnitSelling.Picking;
using UnityEngine;
using Zenject;

namespace UI.Selling
{
    public class SellButtonSwitcher : IDisposable
    {
        private ISwitchableSellButton _sellButton;
        private IPickedSellablePublisher _pickedSellable;
        private IMinion _minion;
        private IBattleContinuingFlag _battleContinuingFlag;

        [Inject]
        private void Construct(ISwitchableSellButton sellButton, IPickedSellablePublisher pickedSellable,
            IBattleContinuingFlag battleContinuingFlag)
        {
            _battleContinuingFlag = battleContinuingFlag;
            _sellButton = sellButton;
            _pickedSellable = pickedSellable;

            _pickedSellable.ElementPicked += OnSellablePicked;
            _pickedSellable.ElementUnpicked += OnSellableUnpicked;
        }

        public void Dispose()
        {
            _pickedSellable.ElementPicked -= OnSellablePicked;
            _pickedSellable.ElementUnpicked -= OnSellableUnpicked;
        }

        private void OnSellableUnpicked(ISellable sellable)
        {
            if ((sellable as MonoBehaviour).transform.parent.GetComponent<IMinion>().Fraction == Fight.Fractions.Fraction.Minions)
            {
                if(HardTutorial.Activated == false)
                    _sellButton.SwitchOff();
            }
        }

        private void OnSellablePicked(ISellable sellable)
        {
            if ((sellable as MonoBehaviour).transform.parent.GetComponent<IMinion>().Fraction == Fight.Fractions.Fraction.Minions)
            {
                if (PlayerPrefs.GetInt("UnitCount") != 1 && _battleContinuingFlag.Value != true)
                {
                    _sellButton.SwitchOn();
                }
                else
                {
                    _sellButton.SwitchOff();
                }
            }
        }
    }
}