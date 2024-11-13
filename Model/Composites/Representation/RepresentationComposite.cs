using System;
using Infrastructure.CompositeDirector.Composites;
using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Representation
{
    public class RepresentationComposite : ProcessComposite<IRepresentation>, IRepresentation
    {
        public override event Action<IProcessExecutor> Disposed;

        public void Represent()
        {
            foreach (IRepresentation item in Items)
            {
                item.Represent();
            }
        }

        protected override void Dispose(bool disposing)
        {
            Disposed?.Invoke(this);
        }

        public override IProcessComposite Clone()
            => new RepresentationComposite();
    }
}