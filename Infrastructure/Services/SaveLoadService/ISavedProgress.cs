using Model.Economy;

namespace Infrastructure.Services.SaveLoadService
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress progress);
    }
}