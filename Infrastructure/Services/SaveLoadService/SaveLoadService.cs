using System.Collections.Generic;
using Model.Economy;
using UnityEngine;

namespace Infrastructure.Services.SaveLoadService
{
    /// <summary>
    /// Сервис сохранений и загрузки
    /// Для того чтобы сохранить данные во время игры классу,который сохраняет какие-то данные необходимо реализовать интерфейс ISavedProgress
    /// и зарегестрироваться в SaveLoadService через метод Register() 
    /// SaveLoadService добавит класс в коллекцию,и когда вызовется метод Save прокинет им текущий прогресс,чтобы классы-реализующие интерфейс проапдейтили его,
    /// после апдейтов сервис сохранит данные в PlayerPrefs
    ///
    ///
    /// Если класс необходимо обновить в зависимости от прогресса при загрузке игры,необходимо реализовать интерфейс ISaveProgressReader
    /// При загрузке нужного уровня будет вызван метод InformProgressReaders() который проинформирует всех подписчиков обновится 
    /// </summary>
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IStorage _progressService;
        private readonly List<ISavedProgressReader> _progressReaders = new List<ISavedProgressReader>(); 
        private readonly List<ISavedProgress> _progressWriters = new List<ISavedProgress>();

        public SaveLoadService(IStorage progressService)
        {
            _progressService = progressService;
        }

        public void Save()
        {
            foreach (ISavedProgress progressWriter in _progressWriters)
                progressWriter.UpdateProgress(_progressService.PlayerProgress);
            
            Debug.Log(_progressService.PlayerProgress.ToJson());
            PlayerPrefs.SetString(ProgressKey, _progressService.PlayerProgress.ToJson());
        }

        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString(ProgressKey)?
                    .ToDeserialized<PlayerProgress>();
        }

        public void Register(ISavedProgress savedProgress)
        {
            _progressWriters.Add(savedProgress);
            _progressReaders.Add(savedProgress);
        }
        public void Register(ISavedProgressReader progressReader)
        {
            _progressReaders.Add(progressReader);
        }

        public void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressWriter in _progressReaders)
                progressWriter.LoadProgress(_progressService.PlayerProgress);
        }

        public void Clear()
        {
            _progressReaders.Clear();
            _progressWriters.Clear();
        }
    }

    public static class DataExtensions
    {
        public static string ToJson(this object obj) =>
                JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
                JsonUtility.FromJson<T>(json);
    }
}