using System;

namespace Battle
{
    public interface IBattleFinishPublisher
    {
        event Action BattleFinished;
        event Action BattleFinished1;
        event Action BattleFinishedAndReadyToMove;
    }
}