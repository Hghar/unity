using Model.Maps.Types;
using UnityEngine;

namespace UI.Map.Mock
{
    public class TileMock : ITile
    {
        private Vector2Int _position;

        public Vector2Int Position => _position;

        public TileMock(int x, int y)
        {
            _position = new Vector2Int(x, y);
        }
    }
}