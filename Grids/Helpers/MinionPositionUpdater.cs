using System;
using System.Collections.Generic;
using Battle;
using Model.Maps;
using Realization.TutorialRealization.Helpers;
using Units;
using UnityEngine;

namespace Grids.Helpers
{
    public class MinionPositionUpdater : IDisposable
    {
        private MinionFactory _minionFactory;
        private IMap _map;
        private IBattleFinishPublisher _battleFinishPublisher;
        private Dictionary<IMinion, Vector2Int> _savedPositions = new();
        private IGrid<IMinion> _grid;
        private IBattleStartPublisher _battleStartPublisher;

        public MinionPositionUpdater(MinionFactory minionFactory, IMap map,
            IBattleFinishPublisher battleFinishPublisher,
            IGrid<IMinion> grid, IBattleStartPublisher battleStartPublisher)
        {
            _grid = grid;
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;
            _map = map;
            _minionFactory = minionFactory;
            _map.Moved += UpdatePositions;
            _battleFinishPublisher.BattleFinished1 += NotSavableUpdatePositions;

            _battleStartPublisher.BeforeBattleStarted += UpdatePositions;

            foreach (var minion in _minionFactory.Minions)
            {
                minion.UpdateWorldPosition(MoveType.AStar);
                _savedPositions.Add(minion, minion.Position);
            }
        }

        private void UpdatePositions()
        {
            _savedPositions.Clear();
            foreach (var minion in _minionFactory.Minions)
            {
                minion.UpdateWorldPosition(MoveType.AStar);
                _savedPositions.Add(minion, minion.Position);
            }
        }
        
        private void NotSavableUpdatePositions()
        {
            foreach (var minion in _minionFactory.Minions)
            {
                if (minion == null || minion.GameObject == null)
                {
                    continue;
                }
                minion.DisableAi();
                SetPlace(minion);
            }
        }
        
        private void SetPlace(IMinion minion)
        {
            if(_savedPositions.ContainsKey(minion) == false)
                return;
            
            if(_grid.Objects[_savedPositions[minion]] == minion)
                return;
            
            if (_grid.Objects[_savedPositions[minion]] != minion && 
                _grid.Objects[_savedPositions[minion]] != null)
            {
                var occupiedMinion = _grid.Objects[_savedPositions[minion]];
                _grid.Swap(minion, occupiedMinion);
                
                minion.UpdateWorldPosition(MoveType.Translate);
                occupiedMinion.UpdateWorldPosition(MoveType.Translate);
                return;
            }
            
            _grid.Place(minion, _savedPositions[minion].x, _savedPositions[minion].y);
            minion.UpdateWorldPosition(MoveType.Translate);
        }

        public void Dispose()
        {
            _map.Moved -= UpdatePositions;
            _battleFinishPublisher.BattleFinished1 -= UpdatePositions;
        }
    }
}