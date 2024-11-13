using Fight.Attack.Projectiles;
using Fight.Damaging;
using Fight.Fractions;
using Point;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class SprayAttackStrategy : MonoBehaviour, IAttackStrategy
    {
        private readonly Quaternion _rotationAddition = Quaternion.Euler(0, 0, 90);

        [SerializeField] private SprayShot _projectilePrefab;
        [SerializeField] private Transform _spawnPoint;

        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion minion)
        {
            SprayShot sprayShot = Instantiate(_projectilePrefab, _spawnPoint.position,
                _spawnPoint.rotation * _rotationAddition);
            sprayShot.Init(damage, fraction);
        }

        public void Init(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint; 
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            return this;
        }
    }
}