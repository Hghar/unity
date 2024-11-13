using System.Collections.Generic;
using Model.Economy;
using UniRx;
using Zenject;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class BiomsCreationData
    {
        public List<CreationData> Bioms;

        public BiomsCreationData(List<CreationData> bioms)
        {
            Bioms = bioms;
        }
    }

    public class CreationData
    {
        public int Index;
        public bool IsLocked;
        public Alignment Alignment;
    }

    public class ChangeData
    {
        public Direction Direction;
        public int MiddleIndex;
    }

    public enum Direction
    {
        Left = -1,
        Right = +1,
    }

    public class BiomItemsViewModel : ViewModel<BiomsCreationData>
    {
        private readonly IStorage _storage;
        private readonly List<MenuItemViewModel> _itemsViewModels;
        public IReadOnlyReactiveProperty<ChangeData> DungeonChanged => _currentDungeon;
        private readonly ReactiveProperty<ChangeData> _currentDungeon = new();

        public IReadOnlyList<MenuItemViewModel> Items => _itemsViewModels;

        public BiomItemsViewModel(IInstantiator instantiator, BiomsCreationData model,IStorage storage) :
                base(model)
        {
            _storage = storage;
            List<MenuItemViewModel> itemsViewModels = new();

            for (int i = 0; i < model.Bioms.Count; i++)
            {
                ItemCreationArgs args = new ItemCreationArgs();
                args.Id = i + 1;
                args.IsBlack = model.Bioms[i].IsLocked;
                args.Alligment = model.Bioms[i].Alignment;
                MenuItemViewModel viewModel = instantiator.Instantiate<MenuItemViewModel>(new object[] { args });
                itemsViewModels.Add(viewModel);
            }

            _itemsViewModels = itemsViewModels;
            _currentDungeon.Value = new ChangeData();
            clickCounter = _storage.PlayerProgress.Bioms.SelectedBiom.Key;
        }

        private int clickCounter;

        public void MoveRight()
        {
            clickCounter--;
            ChangeData data = new ChangeData();
            data.Direction = Direction.Right;
            data.MiddleIndex = clickCounter;
            _currentDungeon.Value = data;
        }

        public void MoveLeft()
        {
            clickCounter++;
            ChangeData data = new ChangeData();
            data.Direction = Direction.Left;
            data.MiddleIndex = clickCounter;
            _currentDungeon.Value = data;
        }
    }
}