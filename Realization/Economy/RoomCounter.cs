using System;
using Battle;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Model.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.Economy
{
    public class RoomCounter:MonoBehaviour
    {
        private IStorage _storage;
        public TMP_Text Counter;
        private IStaticDataService _staticData;
        private BiomeData biomeData;
        private IMap _battleStart;

        [Inject]
        public void Construct(IStorage storage,IMap battleStartPublisher,IStaticDataService staticDataService)
        {
            if(SceneManager.GetActiveScene().name == "FightTest")
                return;
            
            _staticData = staticDataService;
            _battleStart = battleStartPublisher;
            _storage = storage;
            biomeData = _staticData.ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key);
            _battleStart.Moved += UpdateCounter;
            UpdateCounter();
        }

        private int counter;
        private void UpdateCounter()
        {
            counter++;
            Counter.text = $"Room: {counter/2} / {biomeData.Configs[_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber-1]._rooms.Count}";

        }
    }

    
}