using AssetStore.HeroEditor.Common.CommonScripts;
using Infrastructure.Services.WindowService.ViewFactory;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class CoreSittingsView : View<CoreSittingsHierarchy, CoreSittingsModel>
    {
        public CoreSittingsView(CoreSittingsHierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
        {
        }

        protected override void UpdateViewModel(CoreSittingsModel viewModel)
        {
            BindClick(Hierarchy.MainMenuClick, viewModel.OnMenuClick);
            BindClick(Hierarchy.RestartClick, viewModel.OnRestartClick);
            BindClick(Hierarchy.CloseClick, viewModel.OnCloseClick);

        }
    }
    

    public class MenuView : View<MenuHierarchy, MenuViewModel>
    {
        private string WhiteButtonPath = "StaticData/DungeonImages/WhiteButton";
        private string GreenButtonPath = "StaticData/DungeonImages/GreenButton";
        private readonly Sprite white;
        private readonly Sprite green;
        private readonly Sprite greenSprite;
        private readonly Sprite whiteSprite;
        private readonly BiomItemsView _biomItemsView;

        public MenuView(MenuHierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
        {
             green = Resources.Load<Sprite>(GreenButtonPath);
             white = Resources.Load<Sprite>(WhiteButtonPath);
             _biomItemsView = viewFactory.CreateView<BiomItemsView,BiomItemsHierarchy>(hierarchy._biomItemsRoot);
        }

        protected override void UpdateViewModel(MenuViewModel viewModel)
        {
            BindClick(Hierarchy.RightClick, viewModel.OnRightClick);
            BindClick(Hierarchy.LeftClick, viewModel.OnLeftClick);
            BindClick(Hierarchy.Play, viewModel.OnPlay);
            BindClick(Hierarchy.Sitting, viewModel.OnSittingsClick);
            BindClick(Hierarchy.Pumping, viewModel.OnMetaLevelingClick);
            
            Bind(viewModel.CrystalsValue, OnCrystalChange);
            Bind(viewModel.GoldValue, OnGoldChange);
            Bind(viewModel.StageChanged, OnStageChanged);

            OnItemsChange(viewModel.BiomItems);
        }

        private void OnItemsChange(BiomItemsViewModel itemsparent)
        {
            _biomItemsView.Initialize(itemsparent);

        }

        private void OnStageChanged(MenuInfo data)
        {
            Hierarchy.Play.image.sprite = data.Color == Color.white ? green : white;
            Hierarchy.Play.image.color = data.Color;

            Hierarchy.LockImage.enabled = Hierarchy.LockText.enabled = (data.IsLocked && !data.IsLastRight);
            Hierarchy.DungeonName.text = data.DungeonName;
            Hierarchy.LeftClick.SetActive(!data.IsLastLeft);
            Hierarchy.RightClick.SetActive(!data.IsLastRight);
            Hierarchy.Play.enabled = !data.IsLocked;
            Hierarchy.StageText.enabled = !(data.IsLocked && !data.IsLastRight);
            Hierarchy.StageText.text = data.StageText;
        }

        private void OnGoldChange(int value) => 
                Hierarchy.GoldCounter.text = value.ToString();

        private void OnCrystalChange(int value) => 
                Hierarchy.CrystalsCounter.text = value.ToString();
    }
}