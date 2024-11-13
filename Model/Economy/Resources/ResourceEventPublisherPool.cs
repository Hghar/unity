using System;

namespace Model.Economy.Resources
{
    public class ResourceEventPublisherPool
    {
        private readonly IResource[] _resources;

        public ResourceEventPublisherPool(IResource[] resources)
        {
            _resources = resources;
        }

        public INumberEventPublisher TryGetPublisher(Currency currency)
        {
            foreach (IResource resource in _resources)
            {
                if (resource.Currency == currency)
                {
                    return resource;
                }
            }

            throw new Exception($"{currency} not has found");
        }
    }
}