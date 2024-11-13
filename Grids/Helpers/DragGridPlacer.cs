using Fight.Fractions;
using Realization.TutorialRealization.Helpers;
using Units;
using UnityEngine;

namespace Grids.Helpers
{
    public class DragGridPlacer
    {
        private IGrid<IMinion> _grid;
        private MinionFactory _factory;

        public DragGridPlacer(IGrid<IMinion> grid, MinionFactory factory)
        {
            _factory = factory;
            _grid = grid;

            _factory.Created += AddToGrid;
        }

        private void AddToGrid(IMinion minion)
        {
            if(minion.Fraction == Fraction.Enemies)
                return;

            minion.Dragged += () =>
            {
                Place(minion);
            };
        }

        private void Place(IMinion minion)
        {
            Vector2Int closest = _grid.ClosestTo(minion.WorldPosition.x, minion.WorldPosition.y);
            if(closest.x >= 4)
            {
                minion.UpdateWorldPosition(MoveType.Instantly);
                return;
            }

            var status = _grid.Place(minion, minion.WorldPosition.x, minion.WorldPosition.y);
            if (status == PlaceStatus.Occupied && HardTutorial.Activated == false)
            {
                var position = _grid.ClosestTo(minion.WorldPosition.x, minion.WorldPosition.y);
                var occupied = _grid.Get(position.x, position.y);
                _grid.Unbind(minion);
                _grid.Place(occupied, minion.Position.x, minion.Position.y);
                _grid.Place(minion, position.x, position.y);
                occupied.UpdateWorldPosition(MoveType.Instantly);
            }
            minion.UpdateWorldPosition(MoveType.Instantly);
        }

        public void Dispose()
        {
            _factory.Created -= AddToGrid;
        }
    }
}