namespace Ticking
{
    public interface ITickable
    {
        void PreTick(float deltaTime = 1f);
        void Tick(float deltaTime = 1f);
        void LateTick(float deltaTime = 1f);
    }
}