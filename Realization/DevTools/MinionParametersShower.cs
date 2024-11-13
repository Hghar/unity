using System;
using Fight.Fractions;
using TMPro;
using Units;
using UnityEngine;

namespace Realization.DevTools
{
    public class MinionParametersShower : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IMinion _minion;

        private void Start()
        {
            _minion = GetComponentInParent<IMinion>();
        }

        private void Update()
        {
            if (_text == null)
                return;
            _text.text = $"{_minion.EnergyValue}/{_minion.Parameters.Energy.MaxValue}";
        }

        private void OnDrawGizmos()
        {
            if(_minion == null || _minion.Target == null)
                return;

            if (_minion.Fraction == Fraction.Minions)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            if (_minion.Fraction == Fraction.Minions)
            {
                Gizmos.DrawLine(_minion.WorldPosition+Vector2.up*0.1f, 
                    _minion.Target.WorldPosition+Vector2.up*0.1f);
            }
            else
            {
                Gizmos.DrawLine(_minion.WorldPosition, 
                    _minion.Target.WorldPosition);
            }
            
        }
    }
}