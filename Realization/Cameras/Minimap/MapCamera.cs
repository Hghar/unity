using Model.Maps;
using Model.Maps.Types;
using Realization.Configs;
using UnityEngine;
using Zenject;

namespace Realization.Cameras.Minimap
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private IMap _map;
        private Vector2 _roomSize;

        [Inject]
        private void Construct(IMap map, MapConfig mapConfig)
        {
            _roomSize = mapConfig.RoomSize;
            _map = map;
        }

        private void Awake()
        {
            Vector2 left = _map.Tiles[0].Position;
            Vector2 right = _map.Tiles[0].Position;
            Vector2 up = _map.Tiles[0].Position;
            Vector2 down = _map.Tiles[0].Position;

            foreach (ITile tile in _map.Tiles)
            {
                if (tile.Position.x < left.x)
                    left = tile.Position;

                if (tile.Position.x > right.x)
                    right = tile.Position;

                if (tile.Position.y < down.y)
                    down = tile.Position;

                if (tile.Position.y > up.y)
                    up = tile.Position;
            }

            Vector2 middleX = (right + left) * _roomSize / 2;
            Vector2 middleY = (up + down) * _roomSize / 2;

            float lengthX = ((right - left).x + 1) * _roomSize.x;
            float lengthY = ((up - down).y + 1) * _roomSize.y;

            if (lengthX > lengthY)
                _camera.orthographicSize = lengthX * (float) _camera.pixelHeight / (float) _camera.pixelWidth * 0.5f;
            else
                _camera.orthographicSize = lengthY * 0.5f;

            _camera.transform.position = new Vector3(middleX.x, middleY.y, _camera.transform.position.z);
        }
    }
}