using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetStore.HeroEditor.Common.CommonScripts;
using Battle;
using DG.Tweening;
using Entities.ShopItems;
using Fight.Fractions;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Parameters;
using Realization.Configs;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.Shops
{
    public class Shop:ILateTickable,ITickable
    {
        private ShopBehaviour _behaviour;
        private List<ShopItemEntity> _entities;
        private Constants _constants;
        private IStorage _storage;
        private CharacterStore[] _stores;
        private int _level;
        private StoreCharacterChancesConfig[] _characterChances;
        private readonly IStaticDataService _staticDataService;
        private readonly MinionFactory _minionFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Sprite _goldSpite;
        private readonly Sprite _backSpite;

        private const string BackSpritePath = "SpriteAssets/Images/Back";
        private const string GoldSpritePath = "SpriteAssets/Images/Gold";
        public Shop(ShopBehaviour behaviour, ShopItemEntityFactory entityFactory,
                Constants constants, IStorage storage, CharacterStore[] stores,
                StoreCharacterChancesConfig[] characterChances,
                StartBattleButton startBattleButton,
                IStaticDataService staticDataService,
                IEnemiesSpawnedPublisher enemiesSpawnedPublisher,
                MinionFactory minionFactory,
                ICoroutineRunner coroutineRunner
                )
        {
            _goldSpite = Resources.Load<Sprite>(GoldSpritePath);
            _backSpite = Resources.Load<Sprite>(BackSpritePath);

            
            _storage = storage;
            _level = SceneManager.GetActiveScene().name == DefaultNamespace.Constants.FightTestScene
                    ? 0
                    : _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level;
            
            _characterChances = characterChances;
            _staticDataService = staticDataService;
            _minionFactory = minionFactory;
            _coroutineRunner = coroutineRunner;
            _stores = stores;
            _constants = constants;
            _entities = new();
            for (int i = 0; i < 4; i++)
            {
                var entity = entityFactory.Create(new ShopItemEntityFactoryArgs(behaviour.EntitiesParent));
                _entities.Add(entity);
            }
            
            _behaviour = behaviour;
            _behaviour.Reload.onClick.AddListener((ReloadItems));
            if (_level < 4)
                _behaviour.Upgrade.onClick.AddListener((Upgrade));
            else
                _behaviour.Upgrade.interactable = false;
            RepresentUpgrade();

            _behaviour.ReloadPrice.text = $"{_constants.DungeonCostOfStoreUpdate}";
            _behaviour.Open.onClick.AddListener(SwitchEnableStatus);
            startBattleButton.Clicked += CloseShop;
            _behaviour.SittingsButton.onClick.AddListener(CloseShop);
            foreach (var entity in _entities)
            {
                entity.SetGrade(_level);
                entity.UpdateItem();
            }

            UpdateLevelText(); 
            _behaviour.ShopPanel.SetActive(false);

            ColorUtility.TryParseHtmlString("#a6e740", out Color green);
            ColorUtility.TryParseHtmlString("#f0d544", out Color yellow);
            _greenColor = green;
            _yellowColor = yellow;
            
            startBattleButton.Clicked += OnButtonClicked;
            enemiesSpawnedPublisher.EnemiesSpawned += OnEnemiesSpawned;
        }

        private void OnEnemiesSpawned()
        {
            firstRoomPassed++; //Первый спавн по дефолту для первой комнаты,второй спавн когда мы прошли первую комнату
            _coroutineRunner.StartCoroutine(WaitMinionsMoving());

        }
        public IEnumerator WaitMinionsMoving()
        {
            while (NoMinions())
            {
                yield return new WaitForEndOfFrame();
            }

            enableFade = false;
        }

        private bool NoMinions()
        {
            IEnumerable<IMinion> enumerable = _minionFactory.Minions.Where(x=>x.Fraction==Fraction.Minions);
            if (enumerable.Count() == 0) return true;
            return !enumerable.Any((minion => minion.IsMoving));
        }
        private void OnButtonClicked()
        {
            enableFade = true;

        }

        private void CloseShop()
        {
            if (_behaviour.ShopPanel.activeSelf)
                Disable();
        }

        private void Disable()
        {
            _behaviour.ShopPanel.SetActive(false);
            _behaviour.OpenStatus.sprite = _goldSpite;
        }

        private void SwitchEnableStatus()
        {
            if (_behaviour.ShopPanel.activeSelf)
                Disable();
            else
            {
                _behaviour.ArrowShopOpenButton.Disable();
                _behaviour.OpenStatus.sprite = _backSpite;
                _behaviour.ShopPanel.SetActive(true);
            }
        }
        private void RepresentUpgrade()
        {
            if (_level < 4)
                _behaviour.UpgradePrice.text = $"<sprite=0> {_constants.DungeonStoreUpdateCost[_level]}";
            else
                _behaviour.UpgradePrice.text = "";

            PlayUpdateAnimation();

            
            lastChanceProb_1 = _characterChances[_level].Probability_1;
            lastChanceProb_2 = _characterChances[_level].Probability_2;
            lastChanceProb_3 = _characterChances[_level].Probability_3;
            lastChanceProb_4 = _characterChances[_level].Probability_4;
            lastChanceProb_5 = _characterChances[_level].Probability_5;

            _behaviour.Grade1.text = $"{_characterChances[_level].Probability_1}%";
            _behaviour.Grade2.text = $"{_characterChances[_level].Probability_2}%";
            _behaviour.Grade3.text = $"{_characterChances[_level].Probability_3}%";
            _behaviour.Grade4.text = $"{_characterChances[_level].Probability_4}%";
            _behaviour.Grade5.text = $"{_characterChances[_level].Probability_5}%";
            
        }

        private void Upgrade()
        {
            var price = (int)_constants.DungeonStoreUpdateCost[_level];
            if (_storage.HaveResource(Currency.Gold, price))
            {
                var seq = DOTween.Sequence();
                seq.Join(_behaviour.CounterParent.DOScale(1.2f,0.3f));
                seq.Append(_behaviour.CounterParent.DOScale(1f, 0.3f));
                seq.Play();
                
                _storage.SpendResource(Currency.Gold, price);
                _level++;
                _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level = _level;
                foreach (var entity in _entities)
                {
                    entity.SetGrade(_level);
                }

                if (_level == 4)
                {
                    _behaviour.Gold.enabled = false;
                    _behaviour.Upgrade.interactable = false;
                    _behaviour.Helm.enabled = false;
                    _behaviour.Arrow.enabled = false;
                    _behaviour.MaxArrow.gameObject.SetActive(true);
                    _behaviour.MaxText.gameObject.SetActive(true);
                }
                _behaviour.CounterAnimation.Play();
                RepresentUpgrade();
                UpdateLevelText();
            }
        }

        private void PlayUpdateAnimation()
        {
            if (_behaviour.ShopPanel.activeSelf)
            {
                _behaviour.GradeAnimation1.PlayAnimation(lastChanceProb_1 - _characterChances[_level].Probability_1);
                _behaviour.GradeAnimation2.PlayAnimation(lastChanceProb_2 - _characterChances[_level].Probability_2);
                _behaviour.GradeAnimation3.PlayAnimation(lastChanceProb_3 - _characterChances[_level].Probability_3);
                _behaviour.GradeAnimation4.PlayAnimation(lastChanceProb_4 - _characterChances[_level].Probability_4);
                _behaviour.GradeAnimation5.PlayAnimation(lastChanceProb_5 - _characterChances[_level].Probability_5);
                _behaviour.ShopLevelAnimation.PlayAnimation(1);
            }
            else
            {
                _behaviour.ArrowShopOpenButton.Show();
            }
        }

        public void UpdateShopItem(int index, Character config)
        {
            ShopItemEntity shopItemEntity = _entities[index];
            shopItemEntity.SetGrade(config.Level);
            shopItemEntity.UpdateItem(config);
        }
        private void UpdateLevelText()
        {
            _behaviour.UpgradeStatus.text = $"SHOP LVL {_level+1}";
        }

        private void ReloadItems()
        {
            if(_storage.GetResourceValue(Currency.Gold) 
               < (_stores[0].Price +_constants.DungeonCostOfStoreUpdate) && firstRoomPassed < 2 && _level == 0)
                return;
            
            if (_storage.HaveResource(Currency.Gold, _constants.DungeonCostOfStoreUpdate))
            {
                _storage.SpendResource(Currency.Gold, _constants.DungeonCostOfStoreUpdate);
                foreach (var entity in _entities)
                {
                    entity.UpdateItem();
                }
            }
        }
  private Color Color;
        private readonly Color _greenColor;
        private readonly Color _yellowColor;
        private bool enableFade;
        private int firstRoomPassed;
        private float lastChanceProb_1;
        private float lastChanceProb_2;
        private float lastChanceProb_3;
        private float lastChanceProb_4;
        private float lastChanceProb_5;

        public void Tick()
        {
            _behaviour.Fade.enabled = CanBuyUnits() || enableFade;
            _behaviour.Upgrade.interactable = CanUpdateTaven();
        }

        private bool CanUpdateTaven()
        {
            //_staticDataService.CharacterConfig().CharacterStore[0].Price;

            int maxPrice = int.MaxValue;
            for (int i = 0; i < _entities.Count; i++)
                if (maxPrice > _entities[i].BayPrice)
                    maxPrice = _entities[i].BayPrice;

            if (_storage.PlayerProgress.Bioms.CurrentRoom != 1)
                return _storage.HaveResource(Currency.Gold, (int)(_constants.DungeonStoreUpdateCost[_level]));
            else
                return _storage.HaveResource(Currency.Gold, (int)(_constants.DungeonStoreUpdateCost[_level] + maxPrice));
        }

        private bool CanBuyUnits() =>
                MinionFactory.Units.Count >=
                _staticDataService.CharacterConfig().Constants.TavernBasicAmountOfCharacters +
                _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level;

        public void LateTick()
        {
            if (!CanBuyUnits())
            {
                Color = _yellowColor;
            }
            else
            {
                Color = _greenColor;
            }

            _behaviour.MinionCount.color = Color;
            var minionCount = _staticDataService.CharacterConfig().Constants.TavernBasicAmountOfCharacters;
            _behaviour.MinionCount.text =
                    $"{MinionFactory.Units.Count}/{minionCount + _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level}";
        }
        public void AddToQueue(int cellIndex, Character character)
        {
            _entities[cellIndex].AddToQueue(character);
        }
    }
}