using Infrastructure.Services.WindowService.MVVM;
using Model.Economy;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelingCreationData
{
    public List<CreationData> Leveling;

    public LevelingCreationData(List<CreationData> leveling)
    {
        Leveling = leveling;
    }
}

public class CreationData
{
    public Class Class;
    public bool IsLocked;

    public Alignment Alignment;
}

public class ChangeData
{
    public Direction Direction;
    public int MiddleIndex;
}


public class LevelData
{
    public LevelData(Class @class)
    {
        Class = @class;
    }

    public Class Class;
    public int Level = 0;
}
public enum Class
{
    General = 0, Warrior = 1, Priest = 2, Mage = 3, Scout = 4
}

public class MetaLevelingViewModel : ViewModel<LevelingCreationData>
{
    private readonly IStorage _storage;
    private readonly List<LevelingItemViewModel> _itemsViewModels;
    public IReadOnlyReactiveProperty<ChangeData> DungeonChanged => _currentDungeon;
    private readonly ReactiveProperty<ChangeData> _currentDungeon = new();

    public IReadOnlyList<LevelingItemViewModel> Items => _itemsViewModels;

    private LevelData[] levelData = new LevelData[5] 
    {
        new LevelData(Class.General),
        new LevelData(Class.Warrior),
        new LevelData(Class.Priest),
        new LevelData(Class.Mage),
        new LevelData(Class.Scout)
    };

    public MetaLevelingViewModel(IInstantiator instantiator, LevelingCreationData model, IStorage storage) :
            base(model)
    {
        _storage = storage;
        List<LevelingItemViewModel> itemsViewModels = new();

        for (int i = 0; i < model.Leveling.Count; i++)
        {
            ItemCreationArgs args = new ItemCreationArgs();
            args.Id = i + 1;
            args.IsBlack = model.Leveling[i].IsLocked;
            args.Alligment = model.Leveling[i].Alignment;
            LevelingItemViewModel viewModel = instantiator.Instantiate<LevelingItemViewModel>(new object[] { args });
            itemsViewModels.Add(viewModel);
        }

        _itemsViewModels = itemsViewModels;
        _currentDungeon.Value = new ChangeData();
    }

    public void AddLevel(Class @class)
    {
        levelData[(int)@class].Level += 1;
    }

    public void ResetLevel(Class @class)
    {
        levelData[(int)@class].Level = 0;
    }
}
