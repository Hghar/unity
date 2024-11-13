using System;
using System.Collections.Generic;
using Fight;
using Fight.Attack;
using Fight.Damaging;
using Model.Economy;
using UnitSelling;
using UnityEngine;

namespace Parameters
{
    public class UnitParameters : IUnitParameters
    {
        private IHealth _health;
        private Damage _damage;
        private AttackRadius _attackRadius;
        private Armor _armor;
        private Cooldown _cooldown;
        private CriticalChance _chanceOfCriticalDamage;
        private CriticalMultiplier _criticalDamageMultiplier;
        private Agility _agility;
        private Healing _healing;
        private Energy _energy;
        private Level _level;
        private IUnitSellingConfig _sellingConfig;

        public IHealth Health => _health;
        public IDamage Damage => _damage;
        public Agility Agility => _agility;
        public Healing Healing => _healing;
        public AttackRadius AttackRadius => _attackRadius;
        public CriticalChance ChanceOfCriticalDamage => _chanceOfCriticalDamage;
        public CriticalMultiplier CriticalDamageMultiplier => _criticalDamageMultiplier;
        public IUnitSellingConfig CellConfig => _sellingConfig;
        public IArmor Armor => _armor;
        public ICooldown Cooldown => _cooldown;
        public Energy Energy => _energy;

        public Level Level => _level;

        public UnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float chanceOfCriticalDamage, float criticalDamageMultiplier, float agility, int healing, int energy, Level level,
            CurrencyValuePair sellingSetup)
        {
            _health = new Health(health);
            _damage = new Damage(damage);
            _attackRadius = new AttackRadius(attackRadius);
            _armor = new Armor(armor);
            _cooldown = new Cooldown(cooldown);
            _agility = new Agility(agility);
            _healing = new Healing(healing);
            _energy = new Energy(energy);
            _chanceOfCriticalDamage = new CriticalChance(chanceOfCriticalDamage);
            _criticalDamageMultiplier = new CriticalMultiplier(criticalDamageMultiplier);
            _level = level;
            _sellingConfig = new UnitSellingSetup(sellingSetup);
        }

        public UnitParameters UpdateUnitParameters(int health, int damage, int healing, Level level)
        {
            _health.UpdateParam(health);
            _damage.UpdateParam(damage);
            _healing.UpdateParam(healing);

            _level = level;

            return this;
        }


        public bool TryApplyModificator(IParamModificator paramModificator)
        {
            bool anyUnexpectedParam = false;
            
            ApplyParameter(paramModificator, ref anyUnexpectedParam);

            return (anyUnexpectedParam == false);
        }
        
        public bool TryApplyModificators(IReadOnlyList<IParamModificator> paramsModificators)
        {
            bool anyUnexpectedParam = false;
            bool allParamsApplied = false;
            
            foreach (IParamModificator paramModificator in
                     paramsModificators) // TODO: dynamic matching with params decorator 
            {
                allParamsApplied = ApplyParameter(paramModificator, ref anyUnexpectedParam);
            }

            return anyUnexpectedParam && allParamsApplied;
        }

        private bool ApplyParameter(IParamModificator paramModificator, ref bool anyUnexpectedParam)
        {
            bool allParamsApplied = true;
            
            switch (paramModificator.Parameter)
            {
                case ParamType.Health:
                    allParamsApplied &= TryApplyModificator(paramModificator, _health);
                    break;
                case ParamType.Damage:
                    allParamsApplied &= TryApplyModificator(paramModificator, _damage);
                    break;
                case ParamType.AttackRadius:
                    allParamsApplied &= TryApplyModificator(paramModificator, _attackRadius);
                    break;
                case ParamType.Speed:
                    throw new NotImplementedException();
                case ParamType.Armor:
                    allParamsApplied &= TryApplyModificator(paramModificator, _armor);
                    break;
                case ParamType.Cooldown:
                    allParamsApplied &= TryApplyModificator(paramModificator, _cooldown);
                    break;
                case ParamType.RetreatTriggerRadius:
                    throw new NotImplementedException();
                case ParamType.CriticalDamage:
                    allParamsApplied &= TryApplyModificator(paramModificator, ChanceOfCriticalDamage);
                    break;
                case ParamType.CriticalChance:
                    allParamsApplied &= TryApplyModificator(paramModificator, CriticalDamageMultiplier);
                    break;
                case ParamType.Energy:
                    TryApplyModificator(paramModificator, _energy);
                    break;
                default:
                    Debug.LogError($"Unexpected parameter {paramModificator.Parameter}");
                    anyUnexpectedParam = true;
                    break;
            }

            return allParamsApplied;
        }

        private bool TryApplyModificator(IParamModificator paramModificator, IUnitParam param)
        {
            bool isApplyed = param.TryApplyModificator(paramModificator);
            if (isApplyed == false)
                Debug.LogError($"Fail to apply param modificator of {paramModificator.Parameter}");

            return isApplyed;
        }
    }
}