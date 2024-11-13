using System;
using Infrastructure.CompositeDirector.Composites;
using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Savable
{
    public class SavableComposite : ProcessComposite<ISavable>, ISavable
    {
        public override event Action<IProcessExecutor> Disposed;

        public void Save()
        {
            foreach (ISavable item in Items)
            {
                item.Save();
            }
        }

        protected override void Dispose(bool disposing)
        {
            Disposed?.Invoke(this);
        }

        public override IProcessComposite Clone()
            => new SavableComposite();
    }
}