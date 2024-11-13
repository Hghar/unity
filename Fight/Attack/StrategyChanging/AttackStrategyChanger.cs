using System;
using UnityEngine;
#pragma warning disable CS0067

namespace Fight.Attack.StrategyChanging
{
    public class AttackStrategyChanger : MonoBehaviour
    {
        //TODO: rework
        [SerializeField] private MonoBehaviour _attackStrategyObject;

        private IAttackStrategy _attackStrategy;

        public event Action<IAttackStrategy> StrategyChanging;

        public IAttackStrategy AttackStrategy
        {
            get
            {
                if (_attackStrategy != null)
                    return _attackStrategy;

                if (_attackStrategyObject != null && _attackStrategyObject is IAttackStrategy)
                    return _attackStrategyObject as IAttackStrategy;

                return null;
            }
        }

        private void OnValidate()
        {
            // TODO: create common code for this
            if (_attackStrategyObject is IAttackStrategy == false)
            {
                Debug.LogWarning(
                    $"{nameof(_attackStrategyObject)} in {gameObject.name} should be {nameof(IAttackStrategy)}.\n" +
                    $" This field will be null");
                _attackStrategyObject = null;
            }
        }

        private void Awake()
        {
            _attackStrategy = _attackStrategyObject as IAttackStrategy;
        }
    }
}