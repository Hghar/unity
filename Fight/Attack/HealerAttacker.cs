using System;
using System.Collections;
using Fight.Damaging;
using Infrastructure.RayCastingEssence;
using Parameters;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fight.Attack
{
    public class HealerAttacker : Attacker
    {
        private int _attackedCounter = 1;

        public override IEnumerator Attacking()
        {
            var target = _targetFinder.ChosenTarget;
            IMinion newTarget = null;
            float minDistance = float.MaxValue;
            var heal = false;
            float distanceX = 0;
            float distanceY = 0;
            float distance = 0;

            while (target != null &&
                   (_mortality == null || _mortality.IsDying == false))
            {
                if (_attackedCounter >= (_config as HealerUnitParameters).HealCount)
                {
                    _attackedCounter = 0;

                    var minions = Physics2D.OverlapCircleAll(transform.position, 100);
                    foreach (var minionCollider in minions)
                    {
                        var minion = minionCollider.GetComponentInParent<IMinion>();
                        if (minion != null && _targetFinder.ChosenTarget != null)
                        {
                            distanceX = Mathf.Abs(_minion.Position.x - _targetFinder.MinionTarget.Position.x);
                            distanceY = Mathf.Abs(_minion.Position.y - _targetFinder.MinionTarget.Position.y);
                            distance = Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
                            // if (distanceX == distanceY && distanceX == 1)
                            //     distance = 1;

                            if (minion.Fraction != _minion.Fraction)
                                continue;

                            if (newTarget == null && minion.Parameters.Health.Value != minion.Parameters.Health.MaxValue)
                            {
                                newTarget = minion;
                                minDistance = distance;
                                continue;
                            }
                            if (newTarget == null)
                                continue;

                            if (distance > minDistance)
                                continue;

                            if (minion.Parameters.Health.FillPercent <= newTarget.Parameters.Health.FillPercent)
                            {
                                newTarget = minion;
                                minDistance = distance;
                            }
                        }
                    }

                    if (newTarget != null &&
                        newTarget.GameObject != null &&
                        newTarget.GameObject.Equals(null) == false)
                        heal = true;
                }

                yield return new WaitUntil(() =>
                {
                    if (target == null)
                        return false;
                    distanceX = 0;
                    distanceY = 0;

                    if (_targetFinder.ChosenTarget == null || _minion == null)
                    {
                        Debug.Log($"Target is null");

                        return true;
                    }

                    try
                    {
                        distanceX = Mathf.Abs(_minion.Position.x - _targetFinder.MinionTarget.Position.x);
                        distanceY = Mathf.Abs(_minion.Position.y - _targetFinder.MinionTarget.Position.y);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Target is null");
                    }

                    return (distanceX <= _maxDistance && distanceY <= _maxDistance);
                });

                if (_targetFinder.ChosenTarget == null || _minion == null)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);

                yield return new WaitWhile(() => _isWaitingForCooldown);

                yield return new WaitUntil((() => _paused == false));

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

                InvokeAttacked();

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

                if(_minion == null || _targetFinder == null || _targetFinder.MinionTarget == null)
                {
                    //yield return new WaitForSeconds(_cooldown.Value);

                    continue;
                }

                distanceX = Mathf.Abs(_minion.Position.x - _targetFinder.MinionTarget.Position.x);
                distanceY = Mathf.Abs(_minion.Position.y - _targetFinder.MinionTarget.Position.y);
                distance = Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

                if (distanceX <= _maxDistance && distanceY <= _maxDistance)
                {
                    if (newTarget == null ||
                        newTarget.GameObject == null ||
                        newTarget.GameObject.Equals(null))
                    {
                        heal = false;
                    }

                    var minion = (_targetFinder.ChosenTarget as MonoBehaviour).GetComponentInParent<IMinion>();

                    if (heal)
                    {

                        int healValue = _minion.Parameters.Healing.Value;
                        if (Random.Range(0f, 1f) <= _chanceOfCriticalDamage.Value)
                            healValue = (int)(healValue * _criticalDamageMultiplier.Value);

                        newTarget.Heal(healValue);
                        Debug.Log($"{_minion.Name} healed by attacking: {healValue}");
                        heal = false;
                    }
                    else
                    {
                        if (
                            (_targetFinder.ChosenTarget as MonoBehaviour) &&
                            distanceX <= _maxDistance && distanceY <= _maxDistance)
                        {
                            if (minion != null)
                            {
                                if (_attackStrategy != null)
                                {
                                    var oldHealth = minion.Parameters.Health.Value;
                                    _attackStrategy.Attack(damage, _friendlyFractionMarker.Fraction,
                                        minion, _minion);
                                    var newHealth = minion.Parameters.Health.Value;

                                    InvokeAttacked(minion, damage.Value, criticalDamage, (oldHealth - newHealth));
                                }
                            }
                            else
                            {
                                Debug.Log("Minion target == null");

                                //yield return new WaitForSeconds(_cooldown.Value);
                                continue;
                            }
                        }
                    }
                    _attackedCounter++;
                }

                yield return new WaitForSeconds(_cooldown.Value);
            }
        }
    }
}