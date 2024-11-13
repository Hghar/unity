using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Grids.Tests
{
    public class GridTests
    {
        private readonly Vector2Int _size = new(8, 4);
        private GridBehaviour _gridBehaviour;

        [SetUp]
        public void Setup()
        {
            _gridBehaviour = new GameObject().AddComponent<GridBehaviour>();
            _gridBehaviour.CellPrefab = new GameObject().AddComponent<CellBehaviour>();
        }
        
        [Test]
        public void PlaceOnEmptyTile_ReturnsOk()
        {
            // Arrange.
            var grid = new ObjectGrid<IGridObject>(_size, _gridBehaviour);
            var mockObject = new Mock<IGridObject>();

            // Act.
            var status = grid.Place(mockObject.Object, 0, 0);
            var placedObj = grid.Get(0, 0);

            // Assert.
            Assert.AreEqual(PlaceStatus.Ok, status);
            Assert.AreEqual(mockObject.Object, placedObj);
        }
        
        [Test]
        public void PlaceOnTile_ThenChangePosition()
        {
            // Arrange.
            var grid = new ObjectGrid<IGridObject>(_size, _gridBehaviour);
            var mockObject = new Mock<IGridObject>();
            mockObject.SetupAllProperties();

            // Act.
            var status = grid.Place(mockObject.Object, 0, 1);
            var pos0 = mockObject.Object.Position;
            grid.Place(mockObject.Object, 1, 1);
            var pos1 = mockObject.Object.Position;

            // Assert.
            Assert.AreEqual(PlaceStatus.Ok, status);
            Assert.AreEqual(new Vector2Int(0, 1), pos0);
            Assert.AreEqual(new Vector2Int(1, 1), pos1);
        }
        
        [Test]
        public void PlaceOneObjectTwice_ChangePlaceOfIt()
        {
            // Arrange.
            var grid = new ObjectGrid<IGridObject>(_size, _gridBehaviour);
            var mockObject = new Mock<IGridObject>();
            mockObject.SetupAllProperties();

            // Act.
            grid.Place(mockObject.Object, 0, 0);
            var status = grid.Place(mockObject.Object, 1, 1 );
            var empty = grid.Get(0, 0);
            var placed = grid.Get(1, 1);

            // Assert.
            Assert.AreEqual(PlaceStatus.Ok, status);
            Assert.AreEqual(null, empty);
            Assert.AreEqual(mockObject.Object, placed);
        }
        
        [Test]
        public void SetWrongPositionFromAnotherClass_ResetToSavedInGridWithWarning_WhenGet()
        {
            // Arrange.
            var grid = new ObjectGrid<IGridObject>(_size, _gridBehaviour);
            var mockObject = new Mock<IGridObject>();
            mockObject.SetupAllProperties();

            // Act.
            grid.Place(mockObject.Object, 0, 0);
            grid.Place(mockObject.Object, 1, 1 );
            mockObject.Object.Position = new Vector2Int(-1, -1);
            var pos = grid.Get(1, 1).Position;
            
            // Assert.
            Assert.AreEqual(new Vector2Int(1, 1), pos);
        }
    }
}