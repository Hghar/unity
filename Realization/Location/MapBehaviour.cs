using Entities.Tile;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using Model.Maps;
using Model.Maps.Types;
using UnityEngine;
using Zenject;

namespace Realization.Location
{
    public class MapBehaviour : MonoBehaviour
    {
        private TileFactory _factory;
        private Composite<IRepresentation> _representation;
        private IMap _map;

        [Inject]
        private void Construct(Composite<IRepresentation> representation, TileFactory factory, IMap map)
        {
            _map = map;
            _representation = representation;
            _factory = factory;
        }

        private void Awake()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            CleanRepresentations();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            Represent();
        }

        private void Represent()
        {
            CleanRepresentations();

            foreach (ITile tile in _map.Tiles)
            {
                _factory.Create(new TileFactoryArgs(tile));
            }

            _representation.Select().For<PlayableTileEntity>().Do().Represent();
        }

        private void CleanRepresentations()
        {
            _representation.Select().For<PlayableTileEntity>().Do().Dispose();
        }
    }
}