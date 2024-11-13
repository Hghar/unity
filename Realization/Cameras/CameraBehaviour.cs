using DG.Tweening;
using Entities.Tile;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Hidable;
using Model.Composites.Representation;
using Model.Maps;
using Realization.Configs;
using Realization.Utils;
using UnityEngine;
using Zenject;

namespace Realization.Cameras
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _currentTileZone;
        [SerializeField] private float _moveDuration = 2;
        
        private Vector3 _gridOffset;
        private IMap _map;
        private Composite<IRepresentation> _representation;
        private Composite<IHidable> _hidable;
        private Vector2 _roomSize;

        [Inject]
        private void Construct(IMap map, Composite<IRepresentation> representation, Composite<IHidable> hidable,
            MapConfig mapConfig)
        {
            _roomSize = mapConfig.RoomSize;
            _hidable = hidable;
            _representation = representation;
            _map = map;
            _map.Moved += Move;
        }

        private void Awake()
        {
            _gridOffset = _currentTileZone.position;
            
            Camera.main.transform.position = new Vector3(
                _map.Current.Position.x * _roomSize.x,
                _map.Current.Position.y * _roomSize.y,
                transform.position.z);
        }

        private void Start()
        {
            //Move();
        }

        private void OnDestroy()
        {
            _map.Moved -= Move;
        }

        private void Move()
        {
            _hidable.Select().For<PlayableTileEntity>().Do().Hide();

            Vector3 nextPosition = new Vector3(
                _map.Current.Position.x * _roomSize.x,
                _map.Current.Position.y * _roomSize.y,
                transform.position.z);

            _representation.Select().Do().Represent();

            _currentTileZone.position = (Vector2) nextPosition + (Vector2) _gridOffset; // TODO: Replace this
            // TODO: fix error
            Camera.main.transform.DOMove(nextPosition, _moveDuration).OnComplete((() =>
            {
                _representation.Select().ForAll().Except<PlayableTileEntity>().Do().Represent();
            }));
        }
    }
}