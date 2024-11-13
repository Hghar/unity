using System;
using CustomInput.Picking;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units.Picking
{
    public class PickableUnit : MonoBehaviour, IPickableUnit, IPointerClickHandler
    {
        [SerializeField] private Pickable _pickable;
        [SerializeField] private MonoBehaviour _unitObject;

        private IUnit _unit;

        public IUnit Unit => _unit;

        public event Action<IPickableUnit> Picked;
        public event Action<IPickableUnit> Unpicked;
        public event Action<IPickableUnit> Destroying;

        private void OnValidate()
        {
            if (_unitObject == null)
            {
                if (TryGetComponent(out IUnit unit))
                {
                    _unitObject = unit as MonoBehaviour;
                }
                else
                {
                    unit = GetComponentInChildren<IUnit>();
                    if (unit != null)
                    {
                        _unitObject = unit as MonoBehaviour;
                    }
                }
            }
        }

        private void Awake()
        {
            _unit = _unitObject as IUnit;
        }

        private void OnEnable()
        {
            _pickable.Picked += OnPicked;
            _pickable.Unpicked += OnUnpicked;
        }

        private void OnDisable()
        {
            _pickable.Picked -= OnPicked;
            _pickable.Unpicked -= OnUnpicked;
        }

        private void OnDestroy()
        {
            Destroying?.Invoke(this);
        }

        public void Unpick()
        {
            _pickable.Unpick();
        }

        private void OnPicked()
        {
            Picked?.Invoke(this);
        }

        private void OnUnpicked()
        {
            if((_unit as IMinion).GameObject.GetComponentInChildren<Pickable>().Working == false)
                return;
            Unpicked?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}