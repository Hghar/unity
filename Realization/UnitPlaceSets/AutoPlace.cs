using System;
using System.Collections.Generic;
using System.Linq;
using Grids;
using Units;
using UnityEngine;

namespace Realization.UnitplaceSets
{
    public class AutoPlace : IPlaceSet
    {
        private static int[] RowPriority = {1,2,0,3};
        
        private static Dictionary<MinionClass, int[]> ColumnPriority = new()
        {
            {MinionClass.Gladiator, new [] {1,2,3,4}},
            {MinionClass.Templar, new [] {1,2,3,4}},
            {MinionClass.Cleric, new [] {3,2,4,1}},
            {MinionClass.Chanter, new [] {2,1,3,4}},
            {MinionClass.Sorcerer, new [] {4,3,2,1}},
            {MinionClass.Spiritmaster, new [] {3,4,2,1}},
            {MinionClass.Ranger, new [] {4,3,2,1}},
            {MinionClass.Assassin, new [] {2,3,1,4}},
        };

        private List<Vector2Int> _occupiedCells = new ();
        private readonly IGrid<IMinion> _grid;
        private bool _enemy;

        public AutoPlace(IGrid<IMinion> grid, bool enemy)
        {
            _enemy = enemy;
            _grid = grid;
        }
        
        public bool PlaceMinions(params IMinion[] minions)
        {
            try
            {
                _occupiedCells.Clear();
                _grid.Unbind(minions);

                var orderedUnits = MinionsSort(minions);

                //orderedUnits = OrderByPersonalMight(orderedUnits);
                foreach (var unit in orderedUnits)
                {
                    int iterationRow = 0;
                    int iterationColumn = 0;
                    var row = RowPriority[iterationRow];
                    var column = 4-ColumnPriority[unit.Class][iterationColumn];
                    if (_enemy)
                        column = 7 - column;
                
                    while (_grid.Objects[new Vector2Int(column, row)] != null)
                    {
                        iterationRow++;
                        if (iterationRow== RowPriority.Length)
                        {
                            iterationRow = 0;
                            iterationColumn++;
                        }
                        row = RowPriority[iterationRow];
                        column = 4-ColumnPriority[unit.Class][iterationColumn];
                        if (_enemy)
                            column = 7 - column;
                    }
                    _grid.Place(unit, column, row);
                    _occupiedCells.Add(new Vector2Int(column, row));
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private IMinion[] MinionsSort(IMinion[] minions)
        {
            var retMinions = from i in minions orderby i.Might.PersonalMight select i;

            return retMinions.ToArray();
        }

        private IOrderedEnumerable<IMinion> OrderByPersonalMight(IOrderedEnumerable<IMinion> minions)
        {
            //todo order by personal might
            return minions;
        }

        public bool HasPlaces()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_grid.Objects[new Vector2Int(i, j)] == null)
                        return true;
                }
            }

            return false;
        }
    }
}