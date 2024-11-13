using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Entities.ShopItems.Views;
using Fight.Fractions;
using Infrastructure.CompositeDirector.Executors;
using Infrastructure.Shared.Extensions;
using Infrastructure.Shared.Factories;
using Mights;
using Model.Composites.Representation;
using Model.Economy;
using Model.Shops;
using ModestTree.Util;
using Parameters;
using Realization.Configs;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;
using Constants = Realization.Configs.Constants;
using System.Collections;
using System.Threading;
using Infrastructure.Services.StaticData;
using Realization.Shops;

namespace Entities.ShopItems
{
    public class ShopItemEntity : IRepresentation
    {
        public int BayPrice => Price(_character.Grade);

        private readonly ShopItemView _view;
        private IStorage _storage;
        private MinionFactory _factory;
        private Character _character;
        private CharacterConfig _characterConfig;
        private IconsProvider _icons;
        private int _grade;
        private StoreCharacterChancesConfig[] _characterChances;
        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private CancellationTokenSource _cancellationToken = new();

        private InfoHint _infoHint;
        private readonly IStaticDataService _staticDataService;

        private static List<ShopItemEntity> _shopItemEntities = new List<ShopItemEntity>();
        private readonly Queue<Character> _queue = new();

        public ShopItemEntity(ShopItemView view,
                IStorage storage,
                MinionFactory factory,
                CharacterConfig characterConfig,
                IconsProvider icons,
                IBattleStartPublisher startPublisher,
                IBattleFinishPublisher finishPublisher, InfoHint infoHint, IStaticDataService staticDataService)

        {
            _characterChances = characterConfig.StoreСharactersСhances.ToArray();
            _characterConfig = characterConfig;
            _factory = factory;
            _storage = storage;
            _view = view;
            _icons = icons;
            _battleStartPublisher = startPublisher;
            _battleFinishPublisher = finishPublisher;
            _infoHint = infoHint;
            _staticDataService = staticDataService;
            view.Clicked += Buy;
            _battleStartPublisher.BattleStarted += Disable;
            _battleFinishPublisher.BattleFinished += Activate;
            UpdateItem();

            _shopItemEntities.Add(this);

            TickUpdateEffect();
        }

        public event Action<IProcessExecutor> Disposed;

        private void Disable()
        {
            _view._Button.interactable = false;
        }
        
        private void Activate()
        {
            if(_view != null &&
               _view.Equals(null) == false)
            _view._Button.interactable = true;
        }

        public void SetGrade(int grade)
        {
            _grade = grade;
        }

        public void UpdateItem()
        {
            float random = Random.Range(0f, 1f);

            int grade = 1;
            var randoms = new List<float>();
            randoms.Add(_characterChances[_grade].Probability_1/100f);
            randoms.Add(_characterChances[_grade].Probability_2/100f);
            randoms.Add(_characterChances[_grade].Probability_3/100f);
            randoms.Add(_characterChances[_grade].Probability_4/100f);
            randoms.Add(_characterChances[_grade].Probability_5/100f);
            randoms = randoms.OrderBy((f => f)).ToList();
            float cache = 0;
            
            for (int i = 0; i < randoms.Count; i++)
            {
                if (random <= cache+randoms[i])
                {
                    grade = randoms.Count - i;
                    break;
                }
                cache += randoms[i];
            }

            if (_queue.Count == 0)
            {
                _character = _characterConfig.Characters
                    .Where((character =>
                        character.Grade == grade &&
                        character.Uid.Contains("Summoned") == false &&
                        character.Tags.Contains("ally"))).Random();   
            }
            else
            {
                _character = _queue.Dequeue();
            }
            
            Represent();

            _character.Level = 1;
        }
        public void UpdateItem(Character config)
        {
            _character = config;
            Represent();

            _character.Level = 1;
        }

        private void Buy(object sender)
        {
            if (MinionFactory.Units.Count >=
                _staticDataService.CharacterConfig().Constants.TavernBasicAmountOfCharacters +
                _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level)
                return;

            var price = Price(_character.Grade);
            
            if (_storage.HaveResource(Currency.Gold, price))
            {
                _storage.SpendResource(Currency.Gold,price);
                _factory.Create(_character);
                UpdateItem();

                for (int i = 0; i < _shopItemEntities.Count; i++)
                {
                    _shopItemEntities[i].UpdateEffect(true);
                }
            }
        }

        public void Dispose()
        {
            if (_view.Equals(null) == false)
                _view.Dispose();
            
            _cancellationToken.Cancel();
            
            _view.Clicked -= Buy;
            _battleStartPublisher.BattleStarted -= Disable;
            _battleFinishPublisher.BattleFinished -= Activate;
            Disposed?.Invoke(this);
            Disposed = null;
        }

        public void Represent()
        {
            _view.Icon.sprite = _icons.FindIcon(_character.Class, _character.Grade);
            _view.Frame.sprite = _icons.FindFrame(_character.Grade);
            _view.Price.text = Price(_character.Grade).ToString();
            _view.ClassIcon.sprite = _icons.FindClassIcon(_character.Class);

            _view.Grade = _character.Grade;

            _view.Class = (int)_character.Class;

            for (int i = 0; i < _shopItemEntities.Count; i++)
            {
                _shopItemEntities[i].UpdateEffect();
            }
        }

        public async void UpdateEffect(bool isTimer = false)
        {
            if (isTimer)
            {
               // await System.Threading.Tasks.Task.Delay(1000);
            }
            if (_infoHint.IsNewMinion(new CharacterOnSceneInformation(_character.Class, _character.Grade)))
            {
                _view.EffectNewSet(true);
            }
            else
            {
                _view.EffectNewSet(false);
            }

            if (_infoHint.IsNewMinionOnScene(new CharacterOnSceneInformation(_character.Class, _character.Grade))&& MinionFactory.Units.Count>0)
            {
                _view.EffectNewCharacter(true);
            }
            else
            {
                _view.EffectNewCharacter(false);
            }
        }

        private async void TickUpdateEffect()
        {
            while (_cancellationToken.IsCancellationRequested == false)
            {
                await System.Threading.Tasks.Task.Delay(300);

                UpdateEffect();
            }
        }

        private int Price(int characterGrade)
        {
            return _characterConfig.CharacterStore.FirstOrDefault((store => store.Grade == characterGrade)).Price;
        }

        public void AddToQueue(Character character)
        {
            _queue.Enqueue(character);
        }
    }
}