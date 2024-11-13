using AssetStore.HeroEditor.Common.CommonScripts;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.ViewFactory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelingView : View<LevelingMenuHierarchy, LevelingViewModel>
{
    private readonly MetaLevelingView _metaLevelingItemsView;

    public LevelingView(LevelingMenuHierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
    {
        _metaLevelingItemsView = viewFactory.CreateView<MetaLevelingView, LevelingMenuHierarchy>(hierarchy);
    }

    protected override void UpdateViewModel(LevelingViewModel viewModel)
    {
        _metaLevelingItemsView.Initialize(viewModel);

        BindClick(Hierarchy._allLevelingButton, () => { viewModel.OnSelectMenu(Class.General);
            _metaLevelingItemsView.UpdateInfo(viewModel, false); Hierarchy._resetButton.gameObject.SetActive(false);
        }); 

        BindClick(Hierarchy._classButtons[0], () => { viewModel.OnSelectMenu(Class.Warrior);
            _metaLevelingItemsView.UpdateInfo(viewModel, false); Hierarchy._resetButton.gameObject.SetActive(true);
        });
        BindClick(Hierarchy._classButtons[1], () => { viewModel.OnSelectMenu(Class.Priest);
            _metaLevelingItemsView.UpdateInfo(viewModel, false); Hierarchy._resetButton.gameObject.SetActive(true);
        });
        BindClick(Hierarchy._classButtons[2], () => { viewModel.OnSelectMenu(Class.Mage);
            _metaLevelingItemsView.UpdateInfo(viewModel, false); Hierarchy._resetButton.gameObject.SetActive(true);
        });
        BindClick(Hierarchy._classButtons[3], () => { viewModel.OnSelectMenu(Class.Scout);
            _metaLevelingItemsView.UpdateInfo(viewModel, false); Hierarchy._resetButton.gameObject.SetActive(true);
        });

        BindClick(Hierarchy._closeButton, viewModel.OnClose);
        BindClick(Hierarchy._levelUpButton, () => { viewModel.OnLevelUp(_metaLevelingItemsView); _metaLevelingItemsView.UpdateInfo(viewModel, true);
        });
        BindClick(Hierarchy._resetButton, () => { viewModel.OnReset(_metaLevelingItemsView);});
        BindClick(Hierarchy._closeCannotBeReset, () => viewModel.CloseOnReset(_metaLevelingItemsView));

        Bind(viewModel.CrystalsValue, OnCrystalChange);
        Bind(viewModel.GoldValue, OnGoldChange);
        Bind(viewModel.TokkenValue, OnTokkenChange);
    }

    private void OnGoldChange(int value) =>
            Hierarchy._coinText.text = value.ToString();

    private void OnTokkenChange(int value) =>
            Hierarchy._dublonText.text = value.ToString();

    private void OnCrystalChange(int value) =>
            Hierarchy._gemText.text = value.ToString();
}
