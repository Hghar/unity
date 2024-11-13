using System;
using System.Collections.Generic;
using System.Linq;
using AStar;
using AStar.Options;
using Fight.Targeting;
using Google.MiniJSON;
using Grids;
using Realization.Configs;
using Ticking;
using Units;
using UnityEngine;
using ITickable = Ticking.ITickable;

namespace Realization.NewMovers
{
    public class GridMover : ITickable, IDisposable
    {
        private ITickablePool _tickablePool;
        private IGrid<IMinion> _grid;
        private WorldGrid _worldGrid;
        private PathFinderOptions _pathfinderOptions;
        private StandingCounter _standingCounter;

        public GridMover(ITickablePool tickablePool, IGrid<IMinion> gridBehaviour, Constants constants)
        {
            _grid = gridBehaviour;
            _tickablePool = tickablePool;
            _tickablePool.TryAdd(this);
            
            _pathfinderOptions = new PathFinderOptions { 
                PunishChangeDirection = true,
                UseDiagonals = true, 
            };
            
            _worldGrid = new WorldGrid(4, 8);
            _standingCounter = new(constants);
        }

        public void Dispose()
        {
            _tickablePool.TryRemove(this);
        }

        public void PreTick(float deltaTime = 1)
        {
            
        }

        public void Tick(float deltaTime = 1)
        {
            foreach (var minion in _grid.Objects.ToArray())
            {
                if(minion.Value == null)
                    continue;
                if(minion.Value.Target == null)
                    continue;
                if(minion.Value.NeedToMove() == false)
                    continue;

                Position minionPosition = new Position(minion.Value.Position.y, minion.Value.Position.x);
                Position targetPosition = new Position(
                    minion.Value.Target.Position.y, 
                    minion.Value.Target.Position.x);
                
                UpdateWorldGrid();
                _worldGrid[minionPosition.Row, minionPosition.Column] = 1;
                _worldGrid[targetPosition.Row, targetPosition.Column] = 1;
                
                var pathfinder = new PathFinder(_worldGrid, _pathfinderOptions);
                var nextPositions = pathfinder.FindPath(minionPosition, targetPosition);
                
                
                if (HasNextPosition(nextPositions) && minionPosition != nextPositions[1])
                {
                    _grid.Place(minion.Value, nextPositions[1].Column, nextPositions[1].Row);
                }
                else
                {
                    TryFindAnotherAvailableTarget(minion, minionPosition);
                }
            }
        }

        private void TryFindAnotherAvailableTarget(KeyValuePair<Vector2Int, IMinion> minion, Position minionPosition)
        {
            _standingCounter.Standing(minion.Value);
            if (_standingCounter.ReadyToChangeTarget(minion.Value))
            {
                var finder = minion.Value.GameObject.GetComponentInChildren<FightTargetFinder>();
                var targets = FindAvailableTargets(finder, minionPosition);

                finder.FindTargets(targets);
            }
        }

        private List<IMinion> FindAvailableTargets(FightTargetFinder finder, Position minionPosition)
        {
            var targets = finder.CurrentPriorityTargets;
            var availableTargets = new List<IMinion>();
            
            foreach (var target in targets)
            {
                var pathfinder = new PathFinder(_worldGrid, _pathfinderOptions);
                
                Position targetPosition = new Position(
                    target.Position.y, 
                    target.Position.x);
                
                var nextPositions = pathfinder.FindPath(minionPosition, targetPosition);
                if (HasNextPosition(nextPositions))
                    availableTargets.Add(target);
            }

            return availableTargets;
        }

        private static bool HasNextPosition(Position[] nextPos)
        {
            return nextPos.Length >= 2;
        }

        private void UpdateWorldGrid()
        {
            _worldGrid = new WorldGrid(4, 8);

            foreach (var minion in _grid.Objects)
            {
                if(minion.Value != null)
                    _worldGrid[minion.Key.y, minion.Key.x] = 0;
                else
                {
                    try
                    {
                        _worldGrid[minion.Key.y, minion.Key.x] = 1;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"{minion.Key.y} {minion.Key.x}");
                        Debug.LogError(e);
                    }
                }
            }
        }

        public void LateTick(float deltaTime = 1)
        {
            
        }
    }
}