using System.Linq;
using Model.Economy;
using Model.Economy.Resources;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Realization.Economy
{
    public class EconomyBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gold;

        private IStorage _storage;
        private Resource _resource;

        [Inject]
        private void Construct(IStorage storage)
        {
            _storage = storage;
        }

        public void Initialize()
        {
            Subscribe();
            UpdateValue();
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            _resource.Changed -= UpdateValue;
        }

        private void Subscribe()
        {
            _resource = _storage.PlayerProgress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Gold);
            _resource.Changed += UpdateValue;
        }

        private void UpdateValue()
        {
            _gold.text = $" {_storage.GetResourceValue(Currency.Gold)}";
        }

        [Button]
        public void Add10000Gold()
        {
            _storage.AddResource(Currency.Gold, 10000);
        }

        [Button]
        public void Remove10000Gold()
        {
            _storage.SpendResource(Currency.Gold, 10000);
        }
    }
}