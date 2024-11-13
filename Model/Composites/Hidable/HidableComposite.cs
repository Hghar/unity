using System;
using Infrastructure.CompositeDirector.Composites;
using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Hidable
{
    public class HidableComposite : ProcessComposite<IHidable>, IHidable
    {
        public override event Action<IProcessExecutor> Disposed;

        public void Hide()
        {
            foreach (IHidable hidable in Items)
            {
                hidable.Hide();
            }
        }

        public override IProcessComposite Clone()
            => new HidableComposite();

        protected override void Dispose(bool disposing)
        {
            Disposed?.Invoke(this);
        }
    }
}