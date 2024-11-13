using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Grids
{
    public class ObjectGrid<T> :  IGrid<T> where T : class, IGridObject
    {
        private readonly GridBehaviour _behaviour;
        private readonly Dictionary<Vector2Int, T> _objects;
        private Vector2Int _size;

        public IReadOnlyDictionary<Vector2Int, T> Objects => _objects;
        public int Width => _size.x;
        public int Height => _size.y;

        public bool HasEmptyTiles => Width * Height > _objects.Count;

        public ObjectGrid(Vector2Int size, GridBehaviour behaviour)
        {
            _size = size;
            _behaviour = behaviour;
            _behaviour?.InitCells(size);
            

            _objects = new();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    _objects.Add(new Vector2Int(x, y), default);
                }
            }
        }

        public PlaceStatus Place(T obj, int x, int y)
        {
            if(obj == null)
                return PlaceStatus.Error;
                
            var hasPosition = _objects.ContainsValue(obj);
            
            var objectOnPlace = _objects.TryGetValue(new Vector2Int(x, y), out var placedObj);

            if(objectOnPlace == false)
            {
                Debug.LogWarning($"Can't place {obj.Name} to coordinates {x}:{y}. Out of range");
                return PlaceStatus.OutOfRange;
            }

            if (placedObj != null)
            {
                Debug.LogWarning($"Can't place {obj.Name} to coordinates {x}:{y}. Occupied by {placedObj.Name}");
                return PlaceStatus.Occupied;
            }

            if (hasPosition)
                _objects[obj.Position] = default;
            _objects[new Vector2Int(x, y)] = obj;
            obj.Position = new Vector2Int(x, y);
            return PlaceStatus.Ok;
        }

        public PlaceStatus Swap(T first, T second)
        {
            if (_objects.ContainsValue(first) == false ||
                _objects.ContainsValue(second) == false)
                return PlaceStatus.Error;
            
            var firstPosition = GetCoordinates(first);
            var secondPosition = GetCoordinates(second);
            
            Remove(first);
            Place(second, firstPosition.x, firstPosition.y);
            Place(first, secondPosition.x, secondPosition.y);
            return PlaceStatus.Ok;
        }

        public PlaceStatus Place(T obj, float floatX, float floatY)
        {
            var position = _behaviour.FindClosest(new Vector2(floatX, floatY));
            var x = position.x;
            var y = position.y;

            return Place(obj, x, y);
        }

        public T Get(int x, int y)
        {
            var pos = new Vector2Int(x, y);
            
            if (!_objects.TryGetValue(pos, out var placedObj))
                return default;
            if (placedObj == null)
                return default;
            if (placedObj.Position == pos) 
                return placedObj;
                
            Debug.LogWarning($"Not synchronized position detected! " +
                             $"Reset {placedObj.Name} position({placedObj.Position.x}:{placedObj.Position.y}) " +
                             $"to {x}:{y}");
            placedObj.Position = pos;
            return placedObj;
        }

        public Vector2Int GetCoordinates(T target)
            => Objects.FirstOrDefault((pair => pair.Value == target)).Key;

        public Vector3 WorldCoordinates(T obj)
            => _behaviour.Cells[obj.Position].transform.position;

        public void Remove(T obj)
        {
            var coordinates = GetCoordinates(obj);
            _objects[coordinates] = null;
        }

        public Vector2Int ClosestTo(float worldPositionX, float worldPositionY)
        {
            var position = _behaviour.FindClosest(new Vector2(worldPositionX, worldPositionY));
            return position;
        }

        public void Unbind(params T[] obj)
        {
            foreach (var minion in obj)
            {
                if(Has(minion) == false)
                    continue;
                
                var position = GetCoordinates(minion);
                _objects[position] = null;
            }
        }

        public bool Has(T obj)
        {
            return _objects.Any((pair => pair.Value == obj));
        }
    }
}