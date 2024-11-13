using Fight.Damaging;
using Fight.Fractions;
using Model.Maps;
using Helpers.Position;
using Realization.Configs;
using Units;
using UnityEngine;
using Zenject;

namespace Fight.Attack
{
    public class SummonAttackStrategy : MonoBehaviour, IAttackStrategy
    {
        [SerializeField] private Transform _spawnPoint;
        //[SerializeField] private Unit _spawnableUnitPrefab; // TODO: use interface

        private MinionFactory _minionFactory;

        [Inject]
        private void Construct(DiContainer diContainer, IMap map, MapConfig mapConfig) // TODO: remove this dependency
        {
            // _unitFactory = new UnitFactory(diContainer, _spawnableUnitPrefab, map, mapConfig, _spawnPoint);
        }

        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion minion)
        {
            //_unitFactory.Create();
            // TODO: use container
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