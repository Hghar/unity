using Fight;
using Model.Economy;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class Bounty : MonoBehaviour
    {
        [SerializeField] private Mortality _mortality;
        [SerializeField] private Currency _currency;
        [SerializeField] [Min(1)] private int _value;

        private IStorage _storage;

        [Inject]
        private void Construct(IStorage storage)
        {
            _storage = storage;
        }

        private void OnEnable()
        {
            _mortality.Dying += OnDying;
        }

        private void OnDisable()
        {
            _mortality.Dying -= OnDying;
        }

        private void OnDying()
        {
            _storage.AddResource(_currency, _value);
        }
    }
}