using System;
using UnityEngine;

namespace Grids
{
    public class TestSpawner : MonoBehaviour
    {
        [SerializeField] private GridBehaviour _prefab;
        [SerializeField] private TestGridObjectBehaviour _testGridObject;

        private void Awake()
        {
            var grid = new ObjectGrid<TestGridObject>(new Vector2Int(8, 4), _prefab);
            grid.Place(new TestGridObject(_testGridObject), 2, 3);
        }
    }
}