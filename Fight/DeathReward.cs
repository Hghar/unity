using System.Linq;
using DefaultNamespace;
using Infrastructure.Services;
using Infrastructure.Services.RandomService;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Fight
{
    public class DeathReward : MonoBehaviour
    {
        [SerializeField] private Mortality _mortality;
        private IStaticDataService _staticData;
        private IRandomService _randomService;
        private IStorage _storage;
        private float _rewardCoin;

        [Inject]
        public void Construct(IStaticDataService staticDataService, IStorage storage, IRandomService randomService)
        {
            _randomService = randomService;
            _storage = storage;
            _staticData = staticDataService;
        }

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == "FightTest")
                return;
            
            _mortality.Dying += OnDying;
            //DungeonsConfig dungeonsConfig = _staticData.ForDungeons(Constants.DungeonKey + _storage.PlayerProgress.Bioms.CurrentDungeon);
            BiomeData biomeData = _staticData.ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key);
            var dungeonsConfig = biomeData.ForStage(_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber);
            
            CharacterConfig config = _staticData.CharacterConfig();

            RewardCalculator calculator = new RewardCalculator(_randomService);
            int enemyCount = 0;
            foreach (var room in dungeonsConfig._rooms.SkipLast(1))
            {
                enemyCount += room.Lvl_1 + room.Lvl_2 + room.Lvl_3 + room.Lvl_4 + room.Lvl_5;
            }
            
            _rewardCoin = calculator.Calculate(enemyCount, config.Constants.DungeonTargetNumberOfCoins + dungeonsConfig.Coin_Quantity_Modifier ,
                    config.Constants.DungeonCoinsSpread);
            Debug.Log("<color=green>DeathReward Debug : </color>" + _rewardCoin);

        }

        private void OnDestroy()
        {
            _mortality.Dying -= OnDying;
        }

        private void OnDying()
        {
            AddCoins();
        }

        private void AddCoins()
        {
            _storage.AddResource(Currency.Gold, (int)_rewardCoin);
        }

        public void OverrideReward(int newValue)
        {
            _rewardCoin = newValue;
        }
    }

    public class RewardCalculator
    {
        private readonly IRandomService _randomService;

        public RewardCalculator(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public float Calculate(int enemyCount, int dungeonTargetNumberOfCoins, float dungeonCoinsSpread)
        {
            int coinRatePerEnemy =
                    IssueRatePerEnemy(enemyCount, dungeonTargetNumberOfCoins); //норма выдачи монет с противника.
            float minSpread = 1 - dungeonCoinsSpread;
            float maxSpread = 1 + dungeonCoinsSpread;
            float randomSpreed = _randomService.Next(minSpread, maxSpread);
            return CoinRatePerEnemy(coinRatePerEnemy, randomSpreed);
        }

        private float CoinRatePerEnemy(int coinRatePerEnemy, float randomSpreed)
        {
            return coinRatePerEnemy * randomSpreed;
        }

        private int IssueRatePerEnemy(int enemyCount, int dungeonTargetNumberOfCoins)
        {
            return dungeonTargetNumberOfCoins / enemyCount;
        }
    }
}