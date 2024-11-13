using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.WindowService.ViewFactory;
using Model.Economy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class TemporarySolutionForRewards
    {
        private readonly IStorage _storage;

        public TemporarySolutionForRewards(IStorage storage)
        {
            _storage = storage;
        }
        
        public int GoldCount => _gold;
        public int CrystalCount => _crystal;
        public int TokenCount => _token;

        private int _gold;
        private int _crystal;
        private int _token;

        public void Init()
        {
            var biomKey = _storage.PlayerProgress.Bioms.SelectedBiom.Key;
            if (biomKey == 1)
            {
                var stage = _storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber;
                switch (stage)
                {
                    case 1:
                        _crystal = 5;
                        _gold = 100;
                        _token = 0;
                        break;
                    case 2:
                        _crystal = 0;
                        _gold = 100;
                        _token = 5;
                        break;
                    case 3:
                        _crystal = 0;
                        _gold = 135;
                        _token = 8;
                        break;
                    case 4:
                        _crystal = 0;
                        _gold = 190;
                        _token = 12;
                        break;
                    case 5:
                        _crystal = 1;
                        _gold = 250;
                        _token = 16;
                        break;
                }
            }
            if (biomKey == 2)
            {
                var stage = _storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber;
                switch (stage)
                {
                    case 1:
                        _crystal = 0;
                        _gold = 315;
                        _token = 20;
                        break;
                    case 2:
                        _crystal = 0;
                        _gold = 380;
                        _token = 24;
                        break;
                    case 3:
                        _crystal = 0;
                        _gold = 450;
                        _token = 28;
                        break;
                    case 4:
                        _crystal = 0;
                        _gold = 525;
                        _token = 32;
                        break;
                    case 5:
                        _crystal = 1;
                        _gold = 600;
                        _token = 36;
                        break;
                }
            }
        }
    }
    public class WinScreenView : View<WinScreenHierarchy, WinScreenViewModel>
    {
        private readonly IStorage _storage;
        private readonly ISaveLoadService _saveLoadService;
        private List<WinScreenItemHierarchy> _items = new List<WinScreenItemHierarchy>();
        private const string GoldIconPath = "StaticData/WinScreenImages/Gold";
        private const string CrystalIconPath = "StaticData/WinScreenImages/Crystal";
        private const string TokenIconPath = "StaticData/WinScreenImages/Token";
        private WinScreenItemHierarchy Gold;
        private WinScreenItemHierarchy Coin;
        private WinScreenItemHierarchy Token;
        private WinScreenViewModel _viewViewModel;
        private TemporarySolutionForRewards rewards;

        public WinScreenView(WinScreenHierarchy hierarchy, IViewFactory viewFactory,IStorage storage,ISaveLoadService saveLoadService) : base(hierarchy, viewFactory)
        {
            _storage = storage;
            _saveLoadService = saveLoadService;
        }

        protected override void UpdateViewModel(WinScreenViewModel viewViewModel)
        {
            _viewViewModel = viewViewModel;
            BindClick(Hierarchy.CloseClick, viewViewModel.OnCloseClick);
            rewards = new TemporarySolutionForRewards(_storage);
            rewards.Init();
            
            _storage.PlayerProgress.WorldData.CurrencyData.Add(Currency.Crystals,rewards.CrystalCount);
            _storage.PlayerProgress.WorldData.CurrencyData.Add(Currency.Hard,rewards.TokenCount);
            _storage.PlayerProgress.WorldData.CurrencyData.Add(Currency.MetaGold,rewards.GoldCount);
            _saveLoadService.Save();
            if (rewards.GoldCount > 0)
            {
                WinScreenItemHierarchy item = UnityEngine.Object.Instantiate(Hierarchy.Prefab, Hierarchy.Root);
                item.Image.sprite = Resources.Load<Sprite>(GoldIconPath);
                item.Image.rectTransform.sizeDelta = new Vector2(120, 105);

                Gold = item;
                _items.Add(item);
            }

            if (rewards.CrystalCount > 0)
            {
                WinScreenItemHierarchy item = UnityEngine.Object.Instantiate(Hierarchy.Prefab, Hierarchy.Root);
                item.Image.sprite = Resources.Load<Sprite>(CrystalIconPath);
                item.Image.rectTransform.sizeDelta = new Vector2(90, 105);
                Coin = item;

                _items.Add(item);
            }

            if (rewards.TokenCount > 0)
            {
                WinScreenItemHierarchy item = UnityEngine.Object.Instantiate(Hierarchy.Prefab, Hierarchy.Root);
                item.Image.sprite = Resources.Load<Sprite>(TokenIconPath);
                item.Image.rectTransform.sizeDelta = new Vector2(110, 110);

                Token = item;

                _items.Add(item);
            }
            Hierarchy.CloseClick.enabled = false;

        }

        public void OpenAnimation()
        {
            Hierarchy.RewardText.alpha = 0;
            Hierarchy.CloseClick.enabled = false;
            Hierarchy.VictoryImage.localScale = new Vector3(0, 1, 1);
            Hierarchy.ItemsBackground.transform.localScale = new Vector3(1, 0, 1);
            Hierarchy.ContinueText.enabled = false;
            Hierarchy.MainGroup.alpha = 0;

            foreach (var item in _items)
            {
                item.CanvasGroup.alpha = 0;
                item.transform.localScale = Vector3.zero;
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(Hierarchy.MainGroup.DOFade(1, 1.3f).SetEase(Ease.InCirc));
            sequence.Append(Hierarchy.VictoryImage.DOScale(Vector3.one, 1));
            sequence.Append(Hierarchy.ItemsBackground.DOScale(Vector3.one, 0.3f));
            sequence.Append(Hierarchy.RewardText.DOFade(1, 0.1f));


            ScaleReward(sequence);
            sequence.Append(CreateReward());

            sequence.OnComplete(OnMainAnimationEnd);
            sequence.SetEase(Ease.Linear);
        }

        private void ScaleReward(Sequence sequence)
        {
            foreach (var item in _items)
            {
                Sequence itemSequence = DOTween.Sequence();
                itemSequence.Join(item.CanvasGroup.DOFade(1, 1));
                itemSequence.Join(item.transform.DOScale(1, 1));
                sequence.Append(itemSequence);
            }
        }

        private void OnMainAnimationEnd()
        {
            CreateContinueTextAnimation();
            Hierarchy.CloseClick.enabled = true;
        }

        private Sequence CreateReward()
        {
            Sequence reward = DOTween.Sequence();
            int coinCounter = 0;
            int goldCount = 0;
            reward.Join(DOTween.To(() => goldCount, x => goldCount = x, rewards.GoldCount, 1f)
                    .OnUpdate(() => UpdateCounterText(Gold.Counter, goldCount.ToString()))
                    .SetEase(Ease.Linear));

            reward.Join(DOTween.To(() => coinCounter, x => coinCounter = x, rewards.CrystalCount, 1f)
                    .OnUpdate(() => UpdateCounterText(Coin.Counter, coinCounter.ToString()))
                    .SetEase(Ease.Linear));

            reward.Join(DOTween.To(() => coinCounter, x => coinCounter = x, rewards.TokenCount, 1f)
                    .OnUpdate(() => UpdateCounterText(Token.Counter, coinCounter.ToString()))
                    .SetEase(Ease.Linear));
            return reward;
        }


        private void UpdateCounterText(TMP_Text text, string value)
        {
            text.text = value;
        }

        private Tween CreateContinueTextAnimation()
        {
            Hierarchy.ContinueText.enabled = true;

            Hierarchy.ContinueText.alpha = 0;
            Hierarchy.ContinueText.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            float duration = 0.6f;

            Sequence textSequence = DOTween.Sequence();

            Sequence positiveScale = DOTween.Sequence();
            positiveScale.Join(Hierarchy.ContinueText.DOFade(1, duration));
            positiveScale.Join(Hierarchy.ContinueText.transform.DOScale(1f, duration));

            Sequence negativeScale = DOTween.Sequence();
            negativeScale.Join(Hierarchy.ContinueText.DOFade(0, duration));
            negativeScale.Join(Hierarchy.ContinueText.transform.DOScale(0.8f, duration));

            textSequence.Append(positiveScale);
            textSequence.Append(negativeScale);

            textSequence.SetLoops(-1);
            return textSequence;
        }
    }
}