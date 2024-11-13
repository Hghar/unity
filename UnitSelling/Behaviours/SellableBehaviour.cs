using System;
using CustomInput.Picking;
using UnitSelling.Picking;
using UnityEngine;

namespace UnitSelling.Behaviours
{
    public class SellableBehaviour : MonoBehaviour, ISellable
    {
        [SerializeField] private Pickable _pickable;

        public event Action<ISellable> Picked;
        public event Action<ISellable> Unpicked;
        public event Action<ISellable> Destroying;

        public ISellable Sellable => this;
        
        private void Start()
        {
            _pickable.Picked += OnPicked;
            _pickable.Unpicked += OnUnpicked;
        }

        private void OnDestroy()
        {
            _pickable.Picked -= OnPicked;
            _pickable.Unpicked -= OnUnpicked;
            Destroying?.Invoke(this);
        }

        private void OnPicked()
        {
            Picked?.Invoke(this);
        }

        private void OnUnpicked()
        {
            Unpicked?.Invoke(this);
        }

        public void Unpick()
        {
            _pickable.Unpick();
        }
    }
}