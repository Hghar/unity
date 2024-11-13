using System.Collections.Generic;
using Units;

namespace Model.Commands.Parts
{
    public interface IMinionPool
    {
        List<IMinion> Minions { get; }
    }
}