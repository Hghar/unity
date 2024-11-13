using System;
using System.Collections.Generic;
using Events;
using Fight.Attack.Projectiles;
using Fight.Damaging;
using Fight.Fractions;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fight.Attack
{
    public class AutoAttackRangeStrategy : MonoBehaviour, IAttackStrategy
    {
        public GameObject _projectilePrefab;
        [SerializeField] private AttackAnimationArcher _animationArcher;
        [SerializeField] private GameObject _characterParent;

        private Queue<KeyValuePair<IDamage, IMinion>> _hitsList = new();
        private IMinion _caster;
        private Action _onHit;

        private void Start()
        {
            _characterParent.GetComponentInChildren<RangeEventHandler>().ReadyToAttack += () =>
            {
                IDamage damage = _hitsList.Peek().Key;
                IMinion minion = _hitsList.Peek().Value;
                _hitsList.Dequeue();
                Attacker(damage, minion, _caster, _onHit);
            };
        }


        private void Attacker(IDamage damage, IMinion target, IMinion caster, Action onHit)
        {
            GameObject projectile = Object.Instantiate(_projectilePrefab,
                _animationArcher.StartPosBullet,
                Quaternion.identity) as GameObject;
            projectile.GetComponent<ProjectileArcher>().DamageProjectile(target, (int)damage.Value, caster, onHit);
        }

        public void Init(Transform spawnPoint)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion caster)
        {
            _caster = caster;
            _animationArcher.ShootAnimation(() => {});
            _hitsList.Enqueue(new KeyValuePair<IDamage, IMinion>(damage, target));
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            return this;
        }

        public void OnHit(Action onHit)
        {
            _onHit = onHit;
        }
    }
}