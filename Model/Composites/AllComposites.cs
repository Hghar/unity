using System.Collections.Generic;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Hidable;
using Model.Composites.Representation;
using Model.Composites.Savable;

namespace Model.Composites
{
    public static class AllComposites
    {
        public static readonly IReadOnlyList<IProcessComposite> Composites = new List<IProcessComposite>()
        {
            new RepresentationComposite(),
            new SavableComposite(),
            new HidableComposite(),
        };
    }
}