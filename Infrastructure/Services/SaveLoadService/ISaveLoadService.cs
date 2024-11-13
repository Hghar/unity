using Model.Economy;

namespace Infrastructure.Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        public void Save();
        public PlayerProgress LoadProgress();
        public void Register(ISavedProgress savedProgress);
        public void Register(ISavedProgressReader progressReader);
        public void Clear();
        public void InformProgressReaders();
    }
}