using System.Collections.Generic;
using Fight.Attack;
using Parameters;
using UnityEngine;

namespace Units
{
    public interface IMinionItem
    {
        public string JsonCharacterView { get; }
        public IReadOnlyList<IParamModificator> ParamsModificators { get; }
        public MinionClass MinionClass { get; }
        public Sprite Icon { get; }
        public IAttackStrategy AttackStrategy { get; }
    }
}