using Entities.Factories;
using Infrastructure.CompositeDirector;
using Model.Economy.Resources;
using TMPro;

namespace Entities.Resources
{
    public class ResourceEntityFactory : EntityFactory<ResourceEntity, ResourceEntityFactoryArgs>
    {
        public ResourceEntityFactory(CompositeDirector director) : base(director)
        {
        }

        protected override ResourceEntity CreateInternal(ResourceEntityFactoryArgs args)
        {
            return new ResourceEntity(args.View, args.Resource);
        }
    }

    public struct ResourceEntityFactoryArgs
    {
        public IResource Resource { get; }
        public TMP_Text View { get; }

        public ResourceEntityFactoryArgs(IResource resource, TMP_Text view)
        {
            Resource = resource;
            View = view;
        }
    }
}