using System;

namespace Battle
{
    public interface IBattleStartPublisher
    {
        event Action BattleStarted;
        event Action BeforeBattleStarted;
    }
}