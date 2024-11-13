using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StatsBoostService;
using Infrastructure.Services.TickInvokerService;
using Model.Economy;
using Parameters;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using UniRx;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class LevelingViewModel : EmptyViewModel
    {
        public bool IsAnimation = false;

        private readonly IGameStateMachine _gameStateMachine;
        private readonly MetaLevelingWindowPresenter _windowPresenter;
        private readonly IStaticDataService _staticDataService;
        private IStatsBoostService _statsBoostService;
        private IWindowService _windowService;

        private readonly PlayerProgress _progress;
        public IReadOnlyReactiveProperty<int> CrystalsValue => _crystals;
        public IReadOnlyReactiveProperty<int> GoldValue => _gold;
        public IReadOnlyReactiveProperty<int> TokkenValue => _tokken;

        public Class ClassNow = Class.General;

        private readonly ReactiveProperty<int> _crystals = new();
        private readonly ReactiveProperty<int> _gold = new();
        private readonly ReactiveProperty<int> _tokken = new();
        private Class _actualClass = Class.General;

        private string _levelingNameText;

        private int level = 0;

        private MetaLevelingInfo _data;

        public LevelingViewModel(IGameStateMachine gameStateMachine,MetaLevelingWindowPresenter windowPresenter, IStorage storage, IStaticDataService staticDataService, TickInvoker tickInvoker, IStatsBoostService statsBoostService, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _windowPresenter = windowPresenter;
            _staticDataService = staticDataService;
            _progress = storage.PlayerProgress;
            _statsBoostService = statsBoostService;
            _windowService = windowService;

            AddDisposable(tickInvoker.Subscribe(UpdateType.Update, OnUpdated));
            OnUpdated();
            UpdateMenuInfo();
        }

        private void OnUpdated()
        {
            if (isStart)
                AddResTest();

            _crystals.Value = _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Crystals).Value;
            _gold.Value = _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.MetaGold).Value;
            _tokken.Value = _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Hard).Value;
        }

        private bool isStart = false;

        private void AddResTest()
        {
            _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.MetaGold).Add(10000);
            _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Crystals).Add(10000);
            _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Hard).Add(10000);

            isStart = false;
        }

        public void UpdateMenuInfo()
        {
            _data = new MetaLevelingInfo();

            UpdateLevelingMenu(Class.General);
        }

        private float[] MatchStatsMinion(Class @class, int statsCount, int level)
        {
            float[] result = new float[statsCount];

            for (int stats = 0; stats < statsCount; stats++)
            {
                for (int i = 1; i <= level; i++)
                {
                    switch (stats)
                    {
                        case 0:
                            result[stats] += _staticDataService.ForMinionStatsUpgrade(i).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Health;
                            break;
                        case 1:
                            result[stats] += _staticDataService.ForMinionStatsUpgrade(i).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Armor;
                            break;
                        case 2:
                            result[stats] += _staticDataService.ForMinionStatsUpgrade(i).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Power;
                            break;
                    }
                }
            }

            return result;
        }

        private float[] MatchStatsGeneral(int statsCount, int level)
        {
            float[] result = new float[statsCount];

            for (int stats = 0; stats < statsCount; stats++)
            {
                for (int i = 1; i <= level; i++)
                {
                    switch (stats)
                    {
                        case 0:
                            result[stats] += _staticDataService.ForStatsUpgrade(i).Health;
                            break;
                        case 1:
                            result[stats] += _staticDataService.ForStatsUpgrade(i).Armor;
                            break;
                        case 2:
                            result[stats] += _staticDataService.ForStatsUpgrade(i).Power;
                            break;
                        case 3:
                            result[stats] += (_staticDataService.ForStatsUpgrade(i).DodgeChance);
                            break;
                        case 4:
                            result[stats] += (_staticDataService.ForStatsUpgrade(i).CriticalDamageChance);
                            break;
                        case 5:
                            result[stats] += (_staticDataService.ForStatsUpgrade(i).CriticalDamageMultiplier);
                            break;
                        case 6:
                            result[stats] += _staticDataService.ForStatsUpgrade(i).HealPower;
                            break;
                    }
                }
            }

            return result;
        }

        private void UpdateLevelingMenu(Class @class)
        {
            if(_data == null)
            {
                _data = new MetaLevelingInfo();
            }

            _data._class = @class;

            if (@class != Class.General)
            {
                _levelingNameText = $"Bonuses to all {@class.ToString().ToLower() + "s"}";

                _data.ClassInformation = new InformationText[3] {
                    new InformationText("Health"),
                    new InformationText("Power"),
                    new InformationText("Armor") };

                int level = _progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass(@class));

                StatsData statsDataLate = _staticDataService.ForMinionStatsUpgrade(level + 1).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class));

                float[]actualBonusStats = MatchStatsMinion(@class, 3, level);

                _data.ClassInformation[0].WhiteText = actualBonusStats[0] + "%";
                _data.ClassInformation[0].GreenText = statsDataLate.Health + "%";


                _data.ClassInformation[1].WhiteText = actualBonusStats[2] + "%";
                _data.ClassInformation[1].GreenText = statsDataLate.Power + "%";


                _data.ClassInformation[2].WhiteText = actualBonusStats[1] + "%";
                _data.ClassInformation[2].GreenText = statsDataLate.Armor + "%";


                if (level > 0)
                {
                    _data.ClassInformation[0].isUpdate =
                        _staticDataService.ForMinionStatsUpgrade(level).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Health != 0;
                    _data.ClassInformation[2].isUpdate =
                        _staticDataService.ForMinionStatsUpgrade(level).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Power != 0;
                    _data.ClassInformation[1].isUpdate =
                        _staticDataService.ForMinionStatsUpgrade(level).Stats.List.Find(x => x.ID == ClassParentIntoClass(@class)).Armor != 0;
                }

            }
            else
            {
                _levelingNameText = $"Bonuses to all Heroes";

                _data.GeneralInformation = new InformationText[7] 
                {
                    new InformationText("Health"),
                    new InformationText("Armor"),
                    new InformationText("Power"),
                    new InformationText("Chance of dodge"),
                    new InformationText("Critical damage chance"),
                    new InformationText("Critical damage mult."),
                    new InformationText("Power of healing")
                };


                int level = _progress.CoreUpgrades.CurrentGeneralLevel;

                var dataLate = _staticDataService.ForStatsUpgrade(level + 1);

                float[] actualStats = MatchStatsGeneral(7, level);

                _data.GeneralInformation[0].WhiteText = actualStats[0].ToString();
                _data.GeneralInformation[0].GreenText = dataLate.Health.ToString();

                _data.GeneralInformation[1].WhiteText = actualStats[1].ToString();
                _data.GeneralInformation[1].GreenText = dataLate.Armor.ToString();

                _data.GeneralInformation[2].WhiteText = actualStats[2].ToString();
                _data.GeneralInformation[2].GreenText = dataLate.Power.ToString();

                _data.GeneralInformation[3].WhiteText = (actualStats[3] * 100).ToString() + "%";
                _data.GeneralInformation[3].GreenText = (dataLate.DodgeChance * 100).ToString() + "%";

                _data.GeneralInformation[4].WhiteText = (actualStats[4] * 100).ToString() + "%";
                _data.GeneralInformation[4].GreenText = (dataLate.CriticalDamageChance * 100).ToString() + "%";


                _data.GeneralInformation[5].WhiteText = (actualStats[5] * 100).ToString() + "%";
                _data.GeneralInformation[5].GreenText = (dataLate.CriticalDamageMultiplier*100).ToString() + "%";


                _data.GeneralInformation[6].WhiteText = actualStats[6].ToString();
                _data.GeneralInformation[6].GreenText = dataLate.HealPower.ToString();

                if (level > 0)
                {
                    _data.GeneralInformation[0].isUpdate = _staticDataService.ForStatsUpgrade(level).Health != 0f;
                    _data.GeneralInformation[1].isUpdate = _staticDataService.ForStatsUpgrade(level).Armor != 0f;
                    _data.GeneralInformation[2].isUpdate = _staticDataService.ForStatsUpgrade(level).Power != 0f;
                    _data.GeneralInformation[3].isUpdate = _staticDataService.ForStatsUpgrade(level).DodgeChance != 0f;
                    _data.GeneralInformation[4].isUpdate = _staticDataService.ForStatsUpgrade(level).CriticalDamageChance != 0f;
                    _data.GeneralInformation[5].isUpdate = _staticDataService.ForStatsUpgrade(level).CriticalDamageMultiplier != 0f;
                    _data.GeneralInformation[6].isUpdate = _staticDataService.ForStatsUpgrade(level).HealPower != 0f;
                }
            }

            _data.HeadingText = _levelingNameText;

            _data.levels = new int[5]
            {
                _progress.CoreUpgrades.CurrentGeneralLevel,
                _progress.CoreUpgrades.Stats.GetLevel(Units.ClassParent.Warrior),
                _progress.CoreUpgrades.Stats.GetLevel(Units.ClassParent.Priest),
                _progress.CoreUpgrades.Stats.GetLevel(Units.ClassParent.Mage),
                _progress.CoreUpgrades.Stats.GetLevel(Units.ClassParent.Scout)
            };

            OnBay();
        }

        public MetaLevelingInfo OnSelectMenu(Class @class)
        {
            ClassNow = @class;

            UpdateLevelingMenu(@class);

            //_actualClass = new(@class);

            return _data;
        }

        public MetaLevelingInfo ReternSelectMenu()
        {
            OnBay();
            return _data;
        }

        public void OnBay()
        {
            _data.GoldCost = new Cost[5];
            _data.TokkenCost = new Cost[5];

            for (int i = 0; i < _data.GoldCost.Length; i++)
            {
                if (i == 0)
                {
                    _data.GoldCost[0] = new Cost(Currency.Gold,
                                                     _staticDataService.ForStatsUpgrade(_progress.CoreUpgrades.CurrentGeneralLevel + 1).GoldCost,
                                                GoldValue.Value >= _staticDataService.ForStatsUpgrade(_progress.CoreUpgrades.CurrentGeneralLevel + 1).GoldCost);
                }
                else
                {
                    _data.IsCannotBeReset = _progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass((Class)i)) < 1;

                    _data.GoldCost[i] =
                        new Cost(Currency.Gold,
                                 (int)_staticDataService.ForMinionStatsUpgrade(_progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass((Class)i)) + 1).
                                 GoldCost,GoldValue.Value >=
                                 _staticDataService.ForMinionStatsUpgrade(_progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass((Class)i)) + 1).GoldCost);

                    _data.TokkenCost[i] =
                        new Cost(
                            Currency.Hard,
                            (int)_staticDataService.ForMinionStatsUpgrade(_progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass((Class)i)) + 1).TokenCost,
                    TokkenValue.Value >=
                    _staticDataService.ForMinionStatsUpgrade(_progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass((Class)i)) + 1).TokenCost);
                }
            }

            _data.CrystalCost = _staticDataService.CharacterConfig().Constants.GeneralUpgradeResetCost;

            _data.IsBayReset = CrystalsValue.Value >= _staticDataService.CharacterConfig().Constants.GeneralUpgradeResetCost;
        }

        public async void OnLevelUp(MetaLevelingView metaLevelingItemsView)
        {
            if (ClassNow == Class.General)
            {
                _statsBoostService.UpdateStatsAll();
            }
            else
            {
                _statsBoostService.UpdateStats(ClassParentIntoClass(ClassNow));
            }

            UpdateLevelingMenu(ClassNow);

            await System.Threading.Tasks.Task.Delay(10);

            metaLevelingItemsView.UpdateInfo(this, true);
        }

        public void OnClose()
        {
            _windowPresenter.HideWindow();
            _windowPresenter.Dispose();
        }

        public void OnReset(MetaLevelingView metaLevelingView)
        {
            if (_data.IsCannotBeReset = _progress.CoreUpgrades.Stats.GetLevel(ClassParentIntoClass(ClassNow)) < 1)
            {
                metaLevelingView.UpdateInfo(this, false);
                metaLevelingView.CannotBeReset();
            }
            else
            {
                metaLevelingView.UpdateInfo(this, true);
                _statsBoostService.ResetStats(ClassParentIntoClass(ClassNow));
            }

            UpdateLevelingMenu(ClassNow);
        }

        public void CloseOnReset(MetaLevelingView metaLevelingView)
        {
            metaLevelingView.CannotBeResetFast();
        }

        public Units.ClassParent ClassParentIntoClass(Class @class)
        {
            switch (@class)
            {
                case Class.General:
                    return Units.ClassParent.None;
                    break;
                case Class.Warrior:
                    return Units.ClassParent.Warrior;
                    break;
                case Class.Priest:
                    return Units.ClassParent.Priest;
                    break;
                case Class.Mage:
                    return Units.ClassParent.Mage;
                    break;
                case Class.Scout:
                    return Units.ClassParent.Scout;
                    break;
            }

            return Units.ClassParent.None;
        }
    }
}
