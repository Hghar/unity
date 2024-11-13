using Fight.Attack;
using Fight.Damaging;
using Fight.Fractions;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class SpiritmasterAutoAttackRangeStrategy : AutoAttackRangeStrategy
    {
        public override void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion caster)
        {
            base.Attack(damage, fraction, target, caster);
        }
    }
}
