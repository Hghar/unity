using System;
using System.Collections.Generic;
using System.Linq;
using Model.Economy.Resources;
using UnityEngine;

namespace Model.Economy
{
    public class Storage : IStorage, IDisposable
    {
        private IReadOnlyList<Resource> _resources=>PlayerProgress.WorldData.CurrencyData.IResources;
        public PlayerProgress PlayerProgress { get; set; }

        public void Dispose()
        {
            foreach (IResource resource in _resources)
            {
                resource.Dispose();
            }
        }

        public int GetResourceValue(Currency currency)
        {
            if (FindResource(currency, out IResource resource))
                return resource.Value;

            return 0;
        }

        public bool AddResource(Currency currency, int addableAmount, bool animate = true)
        {
            if (FindResource(currency, out IResource resource))
            {
                resource.Add(addableAmount);
                if (currency == Currency.Gold && animate) PlayerProgress.WorldData.CurrencyData.PlusCoreGold(addableAmount);

                return true;
            }

            return false;
        }

        public void SpendResource(Currency currency, int spendableAmount, bool animate = true)
        {
            FindResource(currency, out IResource resource);
            if (currency == Currency.Gold && animate) PlayerProgress.WorldData.CurrencyData.MinusGold(spendableAmount);
            resource.TrySubtract(spendableAmount); // ToDo make a method that will definitely take the value from the currency
        }

        public bool HaveResource(Currency currency, int price)
        {
             FindResource(currency, out IResource resource);
             return resource.CanSubtract(price);
        }

        public void SetResource(Currency currency, int value)
        {
            if (FindResource(currency, out IResource resource))
            {
                resource.Set(value);
            }
        }

        private bool FindResource(Currency currency, out IResource foundResource)
        {
            foundResource = _resources.FirstOrDefault(resource => resource.Currency == currency);

            if (foundResource == null)
            {
                Debug.LogError($"Unexpected {nameof(currency)}");
                return false;
            }

            return true;
        }
    }
}