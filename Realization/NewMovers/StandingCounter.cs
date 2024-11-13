using System.Collections.Generic;
using AStar;
using AStar.Options;
using Helpers.Position;
using Realization.Configs;
using Units;

namespace Realization.NewMovers
{
    public class StandingCounter
    {
        private readonly Dictionary<IMinion, int> _waiting = new();
        private Constants _constants;

        public StandingCounter(Constants constants)
        {
            _constants = constants;
        }
        
        public Position[] FindPath(Position start, Position end,
            WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null)
        {
            var pathfinder = new PathFinder(worldGrid, pathFinderOptions);
            var nextPos = pathfinder.FindPath(start, end);

            return nextPos;
        }

        public void Standing(IMinion minion)
        {
            if (_waiting.ContainsKey(minion))
            {
                _waiting[minion] += 1;
                return;
            }
            _waiting.Add(minion, 1);

            minion.Destroying += Remove;
        }

        private void Remove(IDestroyablePoint obj)
        {
            _waiting.Remove(obj as IMinion);
        }

        public bool ReadyToChangeTarget(IMinion minionValue)
        {
            if (_waiting.ContainsKey(minionValue))
            {
                return _waiting[minionValue] >= _constants.BattleGoalUnavailabilityTickTimer;
            }

            return false;
        }
    }
}