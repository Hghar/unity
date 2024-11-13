using System;
using System.Collections.Generic;
using System.Linq;
using Entities.InventoryItems;
using Entities.ShopItems;
using Fight.Attack;
using Infrastructure.CompositeDirector.Composites;
using Infrastructure.Shared.Extensions;
using Infrastructure.Shared.Factories;
using Model.Composites.Representation;
using Model.Economy;
using Model.Inventories.Items;
using Model.Shops;
using Realization.Configs;
using Realization.Installers;
using Realization.States.CharacterSheet;
using Realization.UI;
using Units;
using UnityEngine;
using Zenject;
#pragma warning disable CS0067 

namespace Entities.Tile.Views
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private PlayableTileView _view;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _shopPanel;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private int _count;
        [SerializeField] private ShopItemConfig[] _configs;
        [SerializeField] private ShopType _shopType;
        [SerializeField] private ResetButton _resetShopPrefab;
        [SerializeField] private UnitDictionary _unitPrefabs;

        private IStorage _storage;
        private ShopItemEntityFactory _shopItemEntityFactory;
        private Composite<IRepresentation> _representation;
        private MinionFactory _minionFactory;
        private List<ShopItemEntity> _entities = new();
        private ResetButton _resetButton;
        private CharacterConfig _characterConfig;
        public ShopType ShopType => _shopType;
        public event Action Clearing;

        // TODO: separate dependencies

        [Inject]
        private void Construct(IStorage storage,
            ShopItemEntityFactory shopItemEntityFactory, Composite<IRepresentation> representation,
            MinionFactory minionFactory,
            CharacterConfig characterConfig)
        {
            _characterConfig = characterConfig;
            _minionFactory = minionFactory;
            _representation = representation;
            _shopItemEntityFactory = shopItemEntityFactory;
            _storage = storage;
        }

        private void Awake()
        {
            _canvas.worldCamera = Camera.main;

            for (int i = 0; i < _count; i++)
            {
                switch (_shopType)
                {
                    case ShopType.Characters:
                        CreateRandomCharacters();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (_shopType == ShopType.Characters)
            {
                if (PlayerPrefs.GetInt("level") != 0)
                {
                    ResetButton resetButton = Instantiate(_resetShopPrefab, _itemsContainer);
                    _resetButton = resetButton;
                    _resetButton.Showed += Reset;
                }
            }

            Canvas.ForceUpdateCanvases();

            _view.Entered += OnEntered;
            _view.Exited += Exited;
            _shopPanel.gameObject.SetActive(false);
        }

        private void Reset()
        {
            foreach (ShopItemEntity entity in _entities)
            {
                entity.Dispose();
            }

            for (int i = 0; i < _count; i++)
            {
                switch (_shopType)
                {
                    case ShopType.Characters:
                        CreateRandomCharacters();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            if (_storage.PlayerProgress.Bioms.CurrentRoom > 2)
            {
                EnergyStorage.IsWorking = false;
            }
        }

        private void OnDestroy()
        {
            _view.Entered -= OnEntered;
            _view.Exited -= Exited;
        }

        private void Exited()
        {
            _shopPanel.gameObject.SetActive(false);
        }

        private void OnEntered()
        {
            _shopPanel.gameObject.SetActive(true);
        }

        private void CreateRandomCharacters()
        {
            try
            {
                // var character = _characterConfig.Characters
                //     .ToList()
                //     .Where((character1 => character1.Tags != null && character1.Tags.Contains("ally"))).Random();
                // var storeConfig = _configs.First((config => config.Name == character.Class.ToString()));
                //
                // FactoryWrapper<Character> wrapper =
                //     new FactoryWrapper<Character>(_minionFactory, character);
                //
                // IShopItem item = new FactoryShopItem(wrapper, _storage, storeConfig.Currency, storeConfig.Name, _characterConfig.CurrentCharacterStorePrice,
                //     storeConfig.Sprite);
                // ShopItemEntityFactoryArgs shopItemEntityFactoryArgs = new ShopItemEntityFactoryArgs(item, _itemsContainer);
                // _entities.Add(_shopItemEntityFactory.Create(shopItemEntityFactoryArgs));
                // _representation.Select().For<ShopItemEntity>().Do().Represent();
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't create unit in shop");
                Debug.LogError(e);
            }
        }
    }
}
