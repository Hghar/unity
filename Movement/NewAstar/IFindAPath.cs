using System.Drawing;

namespace AStar
{
    public interface IFindAPath
    {
        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of positions from the start to end position or empty[] if unreachable</returns>
        Position[] FindPath(Position start, Position end);
    }
}
