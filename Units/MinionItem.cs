using System.Collections.Generic;
using Fight.Attack;
using Parameters;
using UnityEngine;

namespace Units
{
    public class MinionItem : IMinionItem
    {
        private readonly IReadOnlyList<IParamModificator> _paramsModificators;
        private readonly MinionClass _minionClass;
        private readonly Sprite _icon;
        private readonly string _jsonCharacterView;
        private readonly IAttackStrategy _attackStrategy;

        public string JsonCharacterView => _jsonCharacterView;
        public IReadOnlyList<IParamModificator> ParamsModificators => _paramsModificators;
        public MinionClass MinionClass => _minionClass;
        public Sprite Icon => _icon;
        public IAttackStrategy AttackStrategy => _attackStrategy;

        public MinionItem(Sprite icon,
            IReadOnlyList<IParamModificator> paramsModificators,
            MinionClass minionClass,
            string jsonCharacterView,
            IAttackStrategy attackStrategy)
        {
            _jsonCharacterView = jsonCharacterView;
            _icon = icon;
            _paramsModificators = paramsModificators;
            _minionClass = minionClass;
            _attackStrategy = attackStrategy;
        }
    }
}