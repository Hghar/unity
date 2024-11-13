using System;
using System.Collections;
using System.Collections.Generic;
using Fight.Attack;
using Fight.Fractions;
using Firebase.Analytics;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using Model.Economy;
using Model.Maps;
using Model.Maps.Types;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using Realization.General;
using Realization.Installers;
using Units;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class EnemySpawner : MonoBehaviour, IEnemiesSpawnedPublisher
    {
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private LevelEnemies _levelEnemies;
        [SerializeField] private Transform _enemiesContainer;
        [SerializeField] private bool _spawnAtStart = false;

        private readonly List<ITile> _visitedTiles = new List<ITile>(); // TODO: mb this list already exists
        private List<IMinion> _targets = new List<IMinion>();

        private IMap _map;
        private DiContainer _container;
        private Composite<IRepresentation> _representation;
        private IGameStateMachine _game;
        private Coroutine _waitingToWin;
        private IStorage _storage;
        public event Action EnemiesSpawned;

        [Inject]
        private void Construct(DiContainer container, IMap map, Composite<IRepresentation> representation,
            IGameStateMachine game,IStorage storage)
        {
            _game = game;
            _storage = storage;
            _representation = representation;
            _map = map;
            _container = container;
        }

        private void OnEnable()
        {
            if(_map != null)
                _map.Moved += OnMinionsMoved;
            
            if (_spawnAtStart)
            {
                _storage.PlayerProgress.Bioms.SelectedBiom.CurrentRoom = 0;
                OnMinionsMoved();
            }
        }

        private void OnDisable()
        {
            _map.Moved -= OnMinionsMoved;
            EnemiesSpawned = null;
        }

        private void OnMinionsMoved()
        {
            if (_visitedTiles.Contains(_map.Current) || _map.Current is StartTile)
            {
                _representation.Select().ForAll().Do().Represent();
            }

            _visitedTiles.Add(_map.Current);
            if (_map.Current is BossTile)
            {
                // GameObject go = _container.InstantiatePrefab(_levelEnemies.BossWave, _enemiesContainer);
                IMinion boss = _enemyFactory.CreateWithBoss();
                _waitingToWin = StartCoroutine(WaitingToWin(boss, WinGame));
            }
            else
            {
                _targets = new List<IMinion>(_enemyFactory.Create(_storage.PlayerProgress.Bioms.CurrentRoom));
                _waitingToWin = StartCoroutine(WaitingToWin(_targets.ToArray(), WinStandartRoom));
            }
            _storage.PlayerProgress.Bioms.SelectedBiom.CurrentRoom++;

            EnemiesSpawned?.Invoke();
        }

        private IEnumerator WaitingToWin(IMinion enemiesParent, Action callback)
        {
            bool died = false;
            Action<IMinion> action = _ => died = true;
            enemiesParent.Died += action;
            yield return new WaitUntil(() => died);
            //yield return new WaitForEndOfFrame();
            enemiesParent.Died -= action;
            callback?.Invoke();
        }

        private IEnumerator WaitingToWin(IMinion[] enemiesParent, Action callback)
        {
            List<IMinion> diedMinion = new List<IMinion>();

            Action<IMinion> action = (IMinion minion) =>
            {
                diedMinion.Add(minion);
            };

            for (int i = 0; i < enemiesParent.Length; i++)
            {
                enemiesParent[i].Died += action;
            }

            yield return new WaitUntil(() => (diedMinion.Count == enemiesParent.Length));

            for (int i = 0; i < enemiesParent.Length; i++)
            {
                enemiesParent[i].Died -= action;
            }

            callback?.Invoke();
        }

        private void WinGame()
        {
            int index = PlayerPrefs.GetInt("level");
            index = (++index) % 6;

            if (index == 0)
                index++;
            PlayerPrefs.SetInt("level", index);
            FirebaseAnalytics.LogEvent($"win_level_{index}");
            //_game.Invoke(GameEvent.Win);
            _game.Enter<WinState>();
        }

        private void WinStandartRoom()
        {
            for (int i = 0; i <= _targets.Count; i++)
            {
                if (i == _targets.Count)
                {
                    Debug.Log("Win Sandart Room");

                    return;
                }

                if (_targets[i].Fraction == Fraction.Minions)
                {
                    return;
                }
            }
        }
    }
}