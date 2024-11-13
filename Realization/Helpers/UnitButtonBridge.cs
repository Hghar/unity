using System;
using CustomInput.Picking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Realization.Helpers
{
    public class UnitButtonBridge : Button
    {
        [SerializeField] private Pickable _pickable;
        [SerializeField] private Button _button;
        

        private void Awake()
        {
            _pickable.Picked += Pick;
            _pickable.Unpicked += Pick;
        }

        private void OnDestroy()
        {
            _pickable.Picked -= Pick;
            _pickable.Unpicked -= Pick;
        }

        private void Pick()
        {
            _button.onClick.Invoke();
            onClick.Invoke();
        }
    }
}