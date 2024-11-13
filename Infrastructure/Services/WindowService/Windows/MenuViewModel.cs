using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.TickInvokerService;
using Model.Economy;
using Model.Economy.Resources;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using UniRx;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.WindowService.MVVM
{
    public sealed class MenuViewModel : EmptyViewModel
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly TickInvoker _tickInvoker;
        private readonly IInstantiator _instantiator;

        private readonly PlayerProgress _progress;
        public IReadOnlyReactiveProperty<int> CrystalsValue => _crystals;
        public IReadOnlyReactiveProperty<int> GoldValue => _gold;
        public IReadOnlyReactiveProperty<MenuInfo> StageChanged => _stageStatuss;
        private readonly ReactiveProperty<int> _crystals = new();
        private readonly ReactiveProperty<int> _gold = new();
        private readonly ReactiveProperty<MenuInfo> _stageStatuss = new();
        private bool IsLast => _clickCounter - 1 == _staticDataService.BiomsCount;
        private bool IsLocked => _clickCounter > _progress.Bioms.Opened.Count;
        private string _stageText;
        private int _clickCounter = 1;
        public BiomItemsViewModel BiomItems => _biomItemsViewModel;
        private BiomItemsViewModel _biomItemsViewModel;
        private readonly IResource _crystalResource;
        private readonly IResource _goldResource;


        public MenuViewModel(IGameStateMachine gameStateMachine, IStorage storage, IStaticDataService staticDataService,
                IWindowService windowService, TickInvoker tickInvoker, IInstantiator instantiator)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _tickInvoker = tickInvoker;
            _instantiator = instantiator;
            _progress = storage.PlayerProgress;
            _clickCounter = _progress.Bioms.SelectedBiom.Key;
            UpdateDungeon();

            AddDisposable(tickInvoker.Subscribe(UpdateType.Update, OnUpdated));
            UpdateMenuInfo();
            CreateBiomItems();
            _crystalResource = _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.Crystals);
            _goldResource = _progress.WorldData.CurrencyData.IResources.First(x => x.Currency == Currency.MetaGold);
            OnUpdated();

        }

        private void CreateBiomItems()
        {
            List<CreationData> dungeons = new List<CreationData>();
            for (int i = 0; i < _staticDataService.BiomsCount; i++)
            {
                var key = i + 1;
                CreationData creationData = new CreationData();
                creationData.Index = key;
                creationData.Alignment = Alignment.RightInvisible;

                creationData.IsLocked = !(_progress.Bioms.Opened.Count(x => x.Key == key) > 0);
                dungeons.Add(creationData);
            }

            CreationData unknown = new CreationData();
            unknown.Index = _staticDataService.BiomsCount + 1;
            unknown.Alignment = Alignment.RightInvisible;
            dungeons.Add(unknown);
            SetAlligment(dungeons);


            BiomsCreationData biomsCreationData = new BiomsCreationData(dungeons);
            _biomItemsViewModel = _instantiator.Instantiate<BiomItemsViewModel>(new object[] { biomsCreationData });
        }

        private void SetAlligment(List<CreationData> dungeons)
        {
            if (dungeons.Count >= _progress.Bioms.SelectedBiom.Key - 2 && _progress.Bioms.SelectedBiom.Key - 2 >= 0 &&
                _progress.Bioms.SelectedBiom.Key >= 3)
                foreach (var creationData in dungeons.Take(_progress.Bioms.SelectedBiom.Key - 2))
                    creationData.Alignment = Alignment.LeftInvisible;

            if (dungeons.Count >= _progress.Bioms.SelectedBiom.Key - 1 && _progress.Bioms.SelectedBiom.Key - 1 > 0)
                dungeons[_progress.Bioms.SelectedBiom.Key - 2].Alignment = Alignment.Left;

            dungeons[_progress.Bioms.SelectedBiom.Key - 1].Alignment = Alignment.Middle;

            if (dungeons.Count >= _progress.Bioms.SelectedBiom.Key + 1 && _progress.Bioms.SelectedBiom.Key + 1 > 0)
                dungeons[_progress.Bioms.SelectedBiom.Key].Alignment = Alignment.Right;

            if (dungeons.Count > 3)
                dungeons.Last().Alignment = Alignment.RightInvisible;
        }

        private void OnUpdated()
        {
            _crystals.Value = _crystalResource.Value;
            _gold.Value = _goldResource.Value;
        }

        private void UpdateMenuInfo()
        {
            MenuInfo data = new MenuInfo();
            data.StageText = _stageText;
            data.Color = IsLocked || IsLast ? Color.gray : Color.white;
            data.IsLocked = IsLocked || (_clickCounter > _staticDataService.BiomsCount);
            data.IsLastRight = _clickCounter - 1 == _staticDataService.BiomsCount;
            data.IsLastLeft = _clickCounter == 1;
            SetDungeonName(data);
            _stageStatuss.Value = data;
        }

        private void SetDungeonName(MenuInfo data)
        {
            if (!IsLast)
            {
                BiomeData biomeData = _staticDataService.ForBioms(_clickCounter);

                data.DungeonName = $"{_clickCounter}. {biomeData.PrefabName}";
            }
            else
                data.DungeonName = "???";
        }

        public void OnLeftClick()
        {
            if (_clickCounter == 1) return;

            _clickCounter--;
            UpdateDungeon();
            _biomItemsViewModel.MoveRight();
            UpdateMenuInfo();
        }


        public void OnRightClick()
        {
            if (_clickCounter > _staticDataService.BiomsCount) return;
            _clickCounter++;
            UpdateDungeon();
            if (_clickCounter - 1 == _staticDataService.BiomsCount) _stageText = "Coming Soon";

            _biomItemsViewModel.MoveLeft();
            UpdateMenuInfo();
        }

        private void UpdateDungeon()
        {
            if (_staticDataService.BiomsCount < _clickCounter) return;
            var biomeData = _staticDataService.ForBioms(_clickCounter);

            if (_progress.Bioms.Opened.Count(x => x.Key == _clickCounter) > 0)
            {
                var currentStage = _progress.Bioms.Opened.First(x => x.Key == _clickCounter).CompletedStagesCount;
                _stageText =
                        $"Passed stages: {currentStage}/{biomeData.Configs.Count}"; 
            }
        }

        public void OnPlay()
        {
            Biom biom = _progress.Bioms.Opened.First(x => x.Key == _clickCounter);
            _progress.Bioms.SelectedBiom = biom;
            _gameStateMachine.Enter<LoadLevelState, string>(Constants.BattleScene);
        }

        public void OnPumping()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(Constants.Pumping);
        }

        public void OnMetaLeveling()
        {
            _windowService.Open(WindowId.MetaLeveling);
        }

        public void OnSittingsClick()
        {
            _windowService.Open(WindowId.Sittings);
        }

        public void OnMetaLevelingClick()
        {
            _windowService.Open(WindowId.MetaLeveling);
        }
    }
}