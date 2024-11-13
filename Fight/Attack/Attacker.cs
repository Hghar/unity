using System;
using System.Collections;
using System.Collections.Generic;
using Fight.Attack.StrategyChanging;
using Fight.Damaging;
using Fight.Fractions;
using Fight.Targeting;
using Helpers.Position;
using Parameters;
using Realization.Configs;
using Units;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Fight.Attack
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] protected FractionMarker _friendlyFractionMarker;
        [SerializeField] protected FightTargetFinder _targetFinder;
        [SerializeField] private MonoBehaviour _attackStrategyObject;
        [SerializeField] protected Mortality _mortality;
        [SerializeField] private AttackStrategyChanger _strategyChanger;

        [SerializeField] private Level _levelUp;

        private InfoHint _infoHint;
        public IMinion[] minions;

        protected IUnitParameters _config;
        protected ICooldown _cooldown;
        protected IDamage _damage;
        protected CriticalChance _chanceOfCriticalDamage;
        protected CriticalMultiplier _criticalDamageMultiplier;
        protected IAttackStrategy _attackStrategy;
        private Coroutine _attacking;
        protected float _maxDistance;
        protected bool _isWaitingForCooldown;
        private bool _initialized = false;
        protected IMinion _minion;
        protected Constants _constants;
        private bool _isCloseToTarget;
        protected bool _paused;
        private bool _isEndAttack = true;

        public event Action AttackedLegacy;
        public event Action Healing;
        public event Action Ready;
        public event Action Destroyed;
        public event Action<IMinion, float, bool, int> Attacked;
        public event Action<IMinion> Missed;


        public bool IsCloseToTarget
        {
            get
            {
                if (_targetFinder.MinionTarget == null)
                    return true;
                
                var distanceX = Mathf.Abs(_minion.Position.x - _targetFinder.MinionTarget.Position.x);
                var distanceY = Mathf.Abs(_minion.Position.y - _targetFinder.MinionTarget.Position.y);

                _isCloseToTarget = distanceX <= _maxDistance && distanceY <= _maxDistance;
                return _isCloseToTarget;
            }
        }

        [Inject]
        private void Constuct(InfoHint infoHint)
        {
            _infoHint = infoHint;
        }

        private void Start()
        {
            if (_levelUp == null)
            {
                _levelUp = transform.parent.transform.gameObject.GetComponentInChildren<Level>();
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

            if (_attackStrategyObject != null)
                return;

            if (TryGetComponent(out IAttackStrategy attackStrategy))
            {
                _attackStrategyObject = attackStrategy.ToMonoBehaviour();
            }
            else
            {
                attackStrategy = GetComponentInChildren<IAttackStrategy>();
                if (attackStrategy != null)
                {
                    _attackStrategyObject = attackStrategy.ToMonoBehaviour();
                }
            }
        }

        private void OnDestroy()
        {
            if (_targetFinder != null)
                _targetFinder.TargetFound -= OnTargetFound;

            if (_mortality != null)
                _mortality.Dying -= OnDying;

            if (_strategyChanger != null)
                _strategyChanger.StrategyChanging -= OnStrategyChanging;

            _isWaitingForCooldown = false;

            Destroyed?.Invoke();
            AttackedLegacy = null;
            Healing = null;
            Ready = null;
            Destroyed = null;
            Attacked = null;
            Missed = null;
        }

        private void OnDying()
        {
            if (_targetFinder != null)
                _targetFinder.TargetFound -= OnTargetFound;

            if (_mortality != null)
                _mortality.Dying -= OnDying;

            if (_attacking != null)
                StopCoroutine(_attacking);
        }

        private void OnTargetFound()
        {
            StartAttacking();
        }

        private void OnStrategyChanging(IAttackStrategy newStrategy)
        {
            _attackStrategy = newStrategy;
            _attackStrategyObject = newStrategy as MonoBehaviour;
            Debug.Log(newStrategy);
        }

        public virtual IEnumerator Attacking()
        {
            if(_isEndAttack == false)
            {
                yield return new WaitForSeconds(_cooldown.Value);
                _isEndAttack = true;
            }

            _isEndAttack = false;

            IDestroyablePoint minionTarget = _targetFinder.ChosenTarget;

            while (minionTarget != null &&
                   (_mortality == null || _mortality.IsDying == false))
            {
                yield return new WaitUntil(() =>
                {
                    if (minionTarget == null) 
                    {
                        minionTarget = _targetFinder.ChosenTarget;
                        return false;
                    }

                    var distanceX = Mathf.Abs(_minion.Position.x - (minionTarget as MonoBehaviour).GetComponentInParent<IMinion>().Position.x);
                    var distanceY = Mathf.Abs(_minion.Position.y - (minionTarget as MonoBehaviour).GetComponentInParent<IMinion>().Position.y);
                    var distance = Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
                    // if (distanceX == distanceY && distanceX == 1)
                    //     distance = 1;

                    return distanceX <= _maxDistance && distanceY <= _maxDistance;
                });

                yield return new WaitUntil((() => _paused == false));

                _infoHint.Battle(enabled);

                yield return new WaitForSeconds(0.5f);

                yield return new WaitWhile(() => _isWaitingForCooldown);

                if (enabled == false)
                {
                    yield return new WaitUntil(() => enabled);

                    continue;
                }

                var spread = Random.Range(_constants.BattleDamageSpread.x, _constants.BattleDamageSpread.y);
                //0 - attack bonus
                var damageValue = (1 + 0) * (spread * _damage.Value);
                Damage damage;
                bool criticalDamage = false;
                var range = Random.Range(0f, 1f);
                if (range <= _chanceOfCriticalDamage.Value)
                {
                    damage = CriticalDamage(damageValue);
                    criticalDamage = true;
                }
                else
                    damage = new Damage(damageValue);

                // Debug.Log($"{_minion.GameObject.name} calculate damage: {damage.Value}");

                AttackedLegacy?.Invoke();

                switch (_minion.Class)
                {
                    case MinionClass.Gladiator:
                        yield return new WaitForSeconds(0.27f);
                        break;

                    case MinionClass.Assassin:
                        yield return new WaitForSeconds(0.17f);
                        break;

                    case MinionClass.Chanter:
                        yield return new WaitForSeconds(0.17f);
                        break;

                    case MinionClass.Cleric:
                        yield return new WaitForSeconds(0.17f);
                        break;

                    case MinionClass.Ranger:
                        yield return new WaitForSeconds(0.40f);
                        break;

                    case MinionClass.Sorcerer:
                        yield return new WaitForSeconds(0.17f);
                        break;

                    case MinionClass.Spiritmaster:
                        yield return new WaitForSeconds(0.17f);
                        break;

                    case MinionClass.Templar:
                        yield return new WaitForSeconds(0.17f);
                        break;

                }

                if (minionTarget == null)
                {
                    continue;
                }

                var distanceX = Mathf.Abs(_minion.Position.x - (minionTarget as MonoBehaviour).GetComponentInParent<IMinion>().Position.x);
                var distanceY = Mathf.Abs(_minion.Position.y - (minionTarget as MonoBehaviour).GetComponentInParent<IMinion>().Position.y);
                var distance = Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

                var rangeAgility = Random.Range(0f, 1f);

                if (minionTarget == null)
                {
                    continue;
                }

                var minion = (minionTarget as MonoBehaviour).GetComponentInParent<IMinion>();
                bool evaded = false;
                if (rangeAgility <= minion.Parameters.Agility.Value)
                {
                    Debug.Log($"{_minion.GameObject.name} missed damage: {damage.Value}");
                    evaded = true;
                    Missed?.Invoke(minion);
                }

                if (
                    (minionTarget as MonoBehaviour) &&
                    distanceX <= _maxDistance && distanceY <= _maxDistance &&
                    evaded == false)
                {
                    if (minion != null)
                    {
                        if (_attackStrategy != null && _paused == false)
                        {
                            var oldHealth = minion.Parameters.Health.Value;
                            _attackStrategy.Attack(damage, _friendlyFractionMarker.Fraction,
                                minion, _minion);
                            var newHealth = minion.Parameters.Health.Value;

                            if (_attackStrategy is AutoAttackRangeStrategy rangeStrategy)
                            {
                                rangeStrategy.OnHit(() => Attacked?.Invoke(minion, damage.Value, criticalDamage, (oldHealth - newHealth)));
                            }
                            else
                            {
                                Attacked?.Invoke(minion, damage.Value, criticalDamage, (oldHealth - newHealth));
                            }

                            PostDamageEffect(damage, minion);
                        }
                    }
                    else
                    {
                        Debug.Log("Minion target == null");
                    }
                }

                yield return new WaitForSeconds(_cooldown.Value);
            }

            _isEndAttack = true;
        }

        protected void InvokeAttacked()
        {
            AttackedLegacy?.Invoke();
        }
        
        protected void InvokeAttacked(IMinion target, float damage, bool crit, int difference)
        {
            Attacked?.Invoke(target, damage, crit, difference);
        }

        protected void PostDamageEffect(Damage damage, IMinion target)
        {
            if(this.transform.parent.TryGetComponent<Spiritmaster>(out var spiritmaster))
            {
                Heal(damage.Value , spiritmaster, target);
            }
        }

        private void Heal(float damage, Spiritmaster spiritmaster, IMinion target)
        {
            IMinion minion = spiritmaster;
            SpiritmasterUnitParameters thisHeal = (_config as SpiritmasterUnitParameters);

            if (thisHeal != null)
            {
                float damageArmorDifference = damage * (1-target.Parameters.Armor.Value * 0.01f);

                if (damageArmorDifference < 0)
                    damageArmorDifference = 0;
                
                int healing = (int)(damageArmorDifference * thisHeal.HealDamagePercent);
                Debug.Log($"{_minion.GameObject.name} is healing by: {healing}");
                minion.Parameters.Health.Increase(healing);

                Healing?.Invoke();
            }
        }

        protected Damage CriticalDamage(float value)
        {
            Damage damage = new Damage(value);
            ArithmeticAction action =
                new ArithmeticAction(_criticalDamageMultiplier.Value, ArithmeticOperationType.Multiplication);
            IParamModificator modificator = new ParamModificator(action, ParamType.Damage);
            damage.TryApplyModificator(modificator);
            return damage;
        }

        public void StartAttacking()
        {
            if (_attacking != null)
                StopCoroutine(_attacking);

            _attacking = StartCoroutine(Attacking());
        }

        private IEnumerator WaitingForCooldown()
        {
            _isWaitingForCooldown = true;
            yield return new WaitForSeconds(_cooldown.Value);
            _isWaitingForCooldown = false;
        }

        public void SetConfig(IMinion minion, IUnitParameters config, Constants constants)
        {
            if (_initialized)
                return;

            _constants = constants;
            _config = config;
            _minion = minion;
            _cooldown = config.Cooldown;
            _damage = config.Damage;
            _chanceOfCriticalDamage = config.ChanceOfCriticalDamage;
            _criticalDamageMultiplier = config.CriticalDamageMultiplier;

            _attackStrategy = _attackStrategyObject as IAttackStrategy;

            _maxDistance = config.AttackRadius.Value;

            if (_targetFinder != null)
                _targetFinder.TargetFound += OnTargetFound;

            if (_mortality != null)
                _mortality.Dying += OnDying;

            if (_strategyChanger != null)
            {
                _strategyChanger.StrategyChanging += OnStrategyChanging;

                if (_attackStrategyObject == null || (_strategyChanger.AttackStrategy != _attackStrategy))
                {
                    _attackStrategy = _strategyChanger.AttackStrategy;
                    _attackStrategyObject = _attackStrategy as MonoBehaviour;
                    _maxDistance = config.AttackRadius.Value;
                }
            }

            _targetFinder.SetMinion(_minion);

            StartAttacking();
            _initialized = true;
            Ready?.Invoke();
        }

        public void FindNewTarget()
        {
            _targetFinder.FindTargets();
        }

        public void Disable()
        {
            _paused = true;
        }

        public void Activate()
        {
            _paused = false;
        }

        public void UpdateConfig(IUnitParameters config)
        {
            _config = config;
            _cooldown = config.Cooldown;
            _damage = config.Damage;
            _chanceOfCriticalDamage = config.ChanceOfCriticalDamage;
            _criticalDamageMultiplier = config.CriticalDamageMultiplier;

            _attackStrategy = _attackStrategyObject as IAttackStrategy;

            _maxDistance = config.AttackRadius.Value;

            if (_strategyChanger != null)
            {
                if (_attackStrategyObject == null || (_strategyChanger.AttackStrategy != _attackStrategy))
                {
                    _attackStrategy = _strategyChanger.AttackStrategy;
                    _attackStrategyObject = _attackStrategy as MonoBehaviour;
                    _maxDistance = config.AttackRadius.Value;
                }
            }

            _initialized = true;
        }
    }
}