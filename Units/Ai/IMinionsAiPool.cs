namespace Units.Ai
{
    public interface IMinionsAiPool
    {
        public bool TryAdd(IMinionAi minionAI);
        public bool TryRemove(IMinionAi minionAI);
    }
}