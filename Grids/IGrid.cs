using System.Collections.Generic;
using Parameters;
using Units;
using UnityEngine;

namespace Grids
{
    public interface IGrid<T> where T : IGridObject
    {
        IReadOnlyDictionary<Vector2Int, T> Objects { get; }
        int Width { get; }
        int Height { get; }
        bool HasEmptyTiles { get; }

        PlaceStatus Place(T obj, int x, int y);
        PlaceStatus Swap(T first, T second);
        PlaceStatus Place(T obj, float floatX, float floatY);
        T Get(int x, int y);
        Vector2Int GetCoordinates(T obj);
        Vector3 WorldCoordinates(T obj);
        Vector2Int ClosestTo(float worldPositionX, float worldPositionY);
        void Unbind(params T[] obj);
        bool Has(T obj);
    }
}