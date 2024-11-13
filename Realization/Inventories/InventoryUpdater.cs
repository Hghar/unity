using Entities.InventoryItems;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using UnityEngine;
using Zenject;

namespace Realization.Inventories
{
    public class InventoryUpdater : MonoBehaviour
    {
        private InventoryItemEntityFactory _factory;
        private Composite<IRepresentation> _representation;

        [Inject]
        private void Construct(InventoryItemEntityFactory factory, Composite<IRepresentation> representation)
        {
            _factory = factory;
            _representation = representation;

            _factory.Created += OnItemAdded;
        }

        private void OnDestroy()
        {
            _factory.Created -= OnItemAdded;
        }

        private void OnItemAdded()
        {
            _representation.Select().For<InventoryItemEntity>().Do().Represent();
        }
    }
}