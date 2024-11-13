using Model.Economy;

namespace Infrastructure.Services.SaveLoadService
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
}