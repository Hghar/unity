using Fight.Fractions;
using Helpers.Position;
using System;
using System.Collections.Generic;
using Battle;
using Units;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Fight.Targeting
{
    public class TargetContainer : MonoBehaviour
    {
        [SerializeField] private FractionMarker _fractionMarker;
        [SerializeField] private bool _isHealing = false; // TODO: separate

        private readonly List<IMinion> _targets = new List<IMinion>();
        private IBattleStartPublisher _startPublisher;
        private IBattleFinishPublisher _finishPublisher;

        public event Action<IMinion> TargetAdded;
        public event Action<IMinion> TargetRemoved;

        public IEnumerable<IMinion> Targets => _targets;

        // [Inject]
        // private void Construct(IBattleStartPublisher startPublisher, IBattleFinishPublisher finishPublisher)
        // {
        //     _startPublisher = startPublisher;
        //     _finishPublisher = finishPublisher;
        //     _startPublisher.BattleStarted += FindTargets;
        // }
        //
        // private void OnDestroy()
        // {
        //     _startPublisher.BattleStarted -= FindTargets;
        // }

        public void FindTargets()
        {
            var targets = Physics2D.OverlapCircleAll(gameObject.transform.position, 1000);
            _targets.Clear();
            foreach (var collision in targets)
            {
                var target = collision.gameObject.GetComponentInParent<IMinion>();
                if (target != null)
                {
                    if (IsFit(target) && target.IsDestroying == false)
                    {
                        var minion = gameObject.GetComponentInParent<IMinion>();
                        if(minion.Initialized == false || minion.Fraction == target.Fraction)
                            continue;
                        
                        if (_targets.Contains(target) == false)
                        {
                            target.Destroying += OnTargetDestroying;
                            _targets.Add(target);
                            TargetAdded?.Invoke(target);
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // var target = collision.gameObject.GetComponentInParent<IMinion>();
            // if (target != null)
            // {
            //     if (IsFit(target) && target.IsDestroying == false)
            //     {
            //         if (_isHealing && !(IsSameObject(target) == false))
            //             return;
            //
            //         if (_targets.Contains(target) == false)
            //         {
            //             target.Destroying += OnTargetDestroying;
            //             _targets.Add(target);
            //             TargetAdded?.Invoke(target);
            //         }
            //     }
            // }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if (collision.TryGetComponent(out IMinion target))
            // {
            //     if (IsFit(target))
            //     {
            //         if (_isHealing && !(IsSameObject(target) == false))
            //             return;
            //
            //         if (_targets.Contains(target))
            //         {
            //             Remove(target);
            //         }
            //     }
            // }
        }

        private void OnTargetDestroying(IDestroyablePoint target)
        {
            Remove((IMinion) target);
        }

        private void Remove(IMinion target)
        {
            target.Destroying -= OnTargetDestroying;
            _targets.Remove(target);
            TargetRemoved?.Invoke(target);
        }

        private bool IsFit(IMinion target)
        {
            return true;
            // return (target.Fraction == _fractionMarker.Fraction) ^ _isFindingEnemies;
        }

        private bool IsSameObject(IMinion target) // TODO: rework
        {
            // Unit thisUnit = GetComponentInParent<Unit>();
            // if (thisUnit != null)
            // {
            //     ITarget[] thisTargets = thisUnit.GetComponentsInChildren<ITarget>();
            //     if (thisTargets.Contains(target))
            //         return true;
            // }

            return false;
        }

        private bool IsDamaged(IMinion target)
        {
            return target.IsDamage;
        }
    }
}