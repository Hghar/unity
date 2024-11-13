using Moq;
using NUnit.Framework;
using Realization.Configs;
using Realization.NewMovers;
using Ticking;
using Units;
using UnityEngine;

namespace Grids.Tests
{
    public class GridMoverTests
    {
        [Test]
        public void Place_ForwardStep()
        {
            var pool = new TickablePool();
            var size = new Vector2Int(8, 4);
            var grid = new ObjectGrid<IMinion>(size, null);
            var mover = new GridMover(pool, grid, new Constants());
            
            var minion = new Mock<IMinion>();
            var target = new Mock<IMinion>();
            minion.SetupAllProperties();
            target.SetupAllProperties();
            minion.Setup((minion1 => minion1.Target)).Returns(target.Object);
            minion.Setup((minion1 => minion1.NeedToMove())).Returns(true);
            target.Setup((minion1 => minion1.Target)).Returns(minion.Object);
            target.Setup((minion1 => minion1.NeedToMove())).Returns(true);

            grid.Place(minion.Object, 1, 1);
            grid.Place(target.Object, 7, 1);
            mover.Tick();
            
            Assert.AreEqual(new Vector2Int(2, 1), minion.Object.Position);
            Assert.AreEqual(new Vector2Int(6, 1), target.Object.Position);
        }
        
        [Test]
        public void GoingForward_PositionNotWillChange_WhenTargetIsClose()
        {
            var pool = new TickablePool();
            var size = new Vector2Int(8, 4);
            var grid = new ObjectGrid<IMinion>(size, null);
            var mover = new GridMover(pool, grid, new Constants());
            
            var minion = new Mock<IMinion>();
            var target = new Mock<IMinion>();
            minion.SetupAllProperties();
            target.SetupAllProperties();
            minion.Setup((minion1 => minion1.Target)).Returns(target.Object);
            minion.Setup((minion1 => minion1.NeedToMove())).Returns(true);
            target.Setup((minion1 => minion1.Target)).Returns(minion.Object);
            target.Setup((minion1 => minion1.NeedToMove())).Returns(true);

            grid.Place(minion.Object, 1, 1);
            grid.Place(target.Object, 7, 1);
            for (int i = 0; i < 100; i++)
            {
                mover.Tick();
            }
            
            Assert.AreEqual(new Vector2Int(4, 1), minion.Object.Position);
            Assert.AreEqual(new Vector2Int(5, 1), target.Object.Position);
        }
        
        [Test]
        public void GoingForwardWithObstacle_GoAroundObstacle_ByDown()
        {
            var pool = new TickablePool();
            var size = new Vector2Int(8, 4);
            var grid = new ObjectGrid<IMinion>(size, null);
            var mover = new GridMover(pool, grid, new Constants());
            
            var minion = new Mock<IMinion>();
            var obstacle = new Mock<IMinion>();
            var target = new Mock<IMinion>();
            
            minion.SetupAllProperties();
            obstacle.SetupAllProperties();
            target.SetupAllProperties();
            
            minion.Setup((minion1 => minion1.Target)).Returns(target.Object);
            minion.Setup((minion1 => minion1.NeedToMove())).Returns(true);
            obstacle.Setup((minion1 => minion1.Target)).Returns(target.Object);
            target.Setup((minion1 => minion1.Target)).Returns(minion.Object);
            target.Setup((minion1 => minion1.NeedToMove())).Returns(true);

            grid.Place(minion.Object, 1, 1);
            grid.Place(obstacle.Object, 2, 1);
            grid.Place(target.Object, 3, 1);
            for (int i = 0; i < 100; i++)
            {
                mover.Tick();
            }
            
            Assert.AreEqual(new Vector2Int(2, 2), minion.Object.Position);
            Assert.AreEqual(new Vector2Int(2, 1), obstacle.Object.Position);
            Assert.AreEqual(new Vector2Int(3, 1), target.Object.Position);
        }
    }
}