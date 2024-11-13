using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Parameters;
using Units;

namespace Infrastructure.Services.StatsBoostService
{
    public class StatsBoostService : IStatsBoostService
    {
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticDataService;
        private readonly ISaveLoadService _saveLoadService;
        private const Currency Currency = Model.Economy.Currency.MetaGold;
        private const int MaxLevel = 10;
        private CurrencyData CurrencyData => _storage.PlayerProgress.WorldData.CurrencyData;
        private CoreUpgrades CoreUpgrades => _storage.PlayerProgress.CoreUpgrades;

        public StatsBoostService(IStorage storage, IStaticDataService staticDataService,ISaveLoadService saveLoadService)
        {
            _storage = storage;
            _staticDataService = staticDataService;
            _saveLoadService = saveLoadService;
        }

        public void UpdateStatsAll()
        {
            GeneralLevelingUpConfig statsConfig = _staticDataService.ForStatsUpgrade(CoreUpgrades.CurrentGeneralLevel + 1);

            bool canPay = CurrencyData.CanPay(Currency.MetaGold, statsConfig.GoldCost);

            if (canPay && CoreUpgrades.CurrentGeneralLevel < MaxLevel)
            {
                SpendMoney(statsConfig.GoldCost);
                CoreUpgrades.CurrentGeneralLevel++;
                _saveLoadService.Save();
            }
        }

        public void SetUpdateLevel(int level, ClassParent id)
        {
            LevelingUpConfig statsConfig = _staticDataService.ForMinionStatsUpgrade(level);

            CurrencyData.Pay(Currency.MetaGold, (int)statsConfig.GoldCost);
            CurrencyData.Pay(Currency.Hard, (int)statsConfig.TokenCost);
        }

        private void SpendMoney(int price) =>
                CurrencyData.Pay(Currency, price);

        private bool CanUpdate(ClassParent id, int maxLevel) =>
                CoreUpgrades.Stats.GetLevel(id) < maxLevel;

        public void UpdateStats(ClassParent id)
        {
            var level = CoreUpgrades.Stats.GetLevel(id);

            LevelingUpConfig statsConfig = _staticDataService.ForMinionStatsUpgrade(level + 1);

            bool canPay =
                CurrencyData.CanPay(Currency.MetaGold, (int)statsConfig.GoldCost)
                &&
                CurrencyData.CanPay(Currency.Hard, (int)statsConfig.TokenCost);

            if (canPay && CanUpdate(id, MaxLevel))
            {
                SetUpdateLevel(level + 1, id);

                CoreUpgrades.Stats.SetLevel(id, level + 1);
                _saveLoadService.Save();
            }
        }

        public void ResetStats(ClassParent id)
        {
            int resetCost = _staticDataService.CharacterConfig().Constants.GeneralUpgradeResetCost;
            bool canPay = CurrencyData.CanPay(Currency.Crystals, resetCost);
            if (!canPay) return;

            int level = CoreUpgrades.Stats.GetLevel(id);
            CalculateSpentTokens(level, out int tokens);

            CoreUpgrades.Stats.SetLevel(id, 0);
            CurrencyData.Add(Currency.Hard, tokens);
            _saveLoadService.Save();
        }

        private void CalculateSpentTokens(int level,out int  tokens)
        {
            tokens = 0;
            for (int i = 0; i < level; i++)
            {
                LevelingUpConfig config = _staticDataService.ForMinionStatsUpgrade(level);
                tokens += (int)config.TokenCost;
            }
        }

        public bool GeneralLevelIsMax =>
                CoreUpgrades.CurrentGeneralLevel >= MaxLevel;

        public bool LevelIsMax(ClassParent id) =>
                !CanUpdate(id, MaxLevel);
    }
}