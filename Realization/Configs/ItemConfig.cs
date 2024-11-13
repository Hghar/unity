using System.Collections.Generic;
using Fight.Attack;
using Model.Economy;
using Parameters;
using Units;
using UnityEngine;

namespace Realization.Configs
{
    [CreateAssetMenu(fileName = "New Item Config", menuName = "Configs/Item", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField] private int _price = 10;
        [SerializeField] private Currency _currency = Currency.Gold;
        [SerializeField] private List<ParamModificator> _modificators;
        [SerializeField] private MinionClass _class;
        [SerializeField] private string _jsonCharacterView; // TODO: make view more understandable from inspector
        [SerializeField] private MonoBehaviour _attackStrategyPrefab;

        private void OnValidate()
        {
            if (_name != "Unit") // TODO: separate units and inventory-items
            {
                // TODO: create common code for this
                if (_attackStrategyPrefab is IAttackStrategy == false)
                {
                    Debug.LogWarning(
                        $"{nameof(_attackStrategyPrefab)} in {name} should be {nameof(IAttackStrategy)}.\n" +
                        $" This field will be null");
                    _attackStrategyPrefab = null;
                }
            }
        }

        public string JsonCharacterView => _jsonCharacterView;
        public Sprite Sprite => _sprite;
        public string Name => _name;
        public int Price => _price;
        public Currency Currency => _currency;
        public IReadOnlyList<IParamModificator> Modificators => _modificators;
        public MinionClass Class => _class;
        public IAttackStrategy AttackStrategy => _attackStrategyPrefab as IAttackStrategy;
    }
}