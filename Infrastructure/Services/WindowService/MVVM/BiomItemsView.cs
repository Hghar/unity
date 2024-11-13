using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService.ViewFactory;
using Parameters;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public sealed class BiomItemsView : View<BiomItemsHierarchy, BiomItemsViewModel>
    {
        private Dictionary<int, MenuItemViewHierarchy> _dungeons;
        private readonly IStaticDataService _staticDataService;
        private string DungeonIconFolderPath = "StaticData/DungeonImages/";
        private string UnknownIconPath = "StaticData/DungeonImages/Question";
        private string BlackFilterPath = "StaticData/DungeonImages/BlackFilter/";
        private const int Duration = 1;


        public BiomItemsView(BiomItemsHierarchy hierarchy, IViewFactory viewFactory,
                IStaticDataService staticDataService) : base(hierarchy, viewFactory)
        {
            _staticDataService = staticDataService;
        }

        protected override void UpdateViewModel(BiomItemsViewModel viewModel)
        {
            Dictionary<int, MenuItemViewHierarchy> items = new Dictionary<int, MenuItemViewHierarchy>();
            foreach (var item in viewModel.Items.SkipLast(1)) //skip unknowed
            {
                BiomeData dungeonsConfig = _staticDataService.ForBioms(item.Id);
                ConfigBiom forStage = dungeonsConfig.ForStage(1); //can be any stage 
                string path = item.IsBlack
                        ? BlackFilterPath + forStage.PrefabName
                        : DungeonIconFolderPath + forStage.PrefabName;
                MenuItemViewHierarchy dungeonView = Object.Instantiate(Hierarchy.Prefab, Hierarchy.Root);
                dungeonView.gameObject.name = $"{item.Id}";
                dungeonView.Image.sprite = Resources.Load<Sprite>(path);
                dungeonView.transform.position =  Hierarchy.AlligmentHelper.GetPosition(item.Alignment);
                items.Add(item.Id, dungeonView);
            }

            MenuItemViewHierarchy view = Object.Instantiate(Hierarchy.Prefab, Hierarchy.Root);
            view.Image.sprite = Resources.Load<Sprite>(UnknownIconPath);
            MenuItemViewModel unknownLast = viewModel.Items.Last();
            view.transform.position = Hierarchy.AlligmentHelper.GetPosition(unknownLast.Alignment);
            view.Image.color = Color.black;
            items.Add(unknownLast.Id, view);

            Bind(viewModel.DungeonChanged, OnDungeonChanged);

            _dungeons = items;
        }

        private void OnDungeonChanged(ChangeData index)
        {
            if (index.Direction == Direction.Left)
            {
                if (index.MiddleIndex > 0 && index.MiddleIndex <= _dungeons.Count)
                {
                    if (_dungeons.ContainsKey(index.MiddleIndex - 1) && _dungeons.ContainsKey(index.MiddleIndex))
                        _dungeons[index.MiddleIndex]
                                .transform
                                .DOMove(GetPosition(Alignment.Middle), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex - 1))
                        _dungeons[index.MiddleIndex - 1]
                                .transform
                                .DOMove(GetPosition(Alignment.Left), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex + 1))
                        _dungeons[index.MiddleIndex + 1]
                                .transform
                                .DOMove(GetPosition(Alignment.Right), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex - 2))
                        _dungeons[index.MiddleIndex - 2]
                                .transform
                                .DOMove(GetPosition(Alignment.LeftInvisible), Duration);
                }
            }

            if (index.Direction == Direction.Right)
            {
                if (index.MiddleIndex < _dungeons.Count)
                {
                    if (_dungeons.ContainsKey(index.MiddleIndex + 1) && _dungeons.ContainsKey(index.MiddleIndex))
                        _dungeons[index.MiddleIndex]
                                .transform
                                .DOMove(GetPosition(Alignment.Middle), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex + 1))
                        _dungeons[index.MiddleIndex + 1]
                                .transform
                                .DOMove(GetPosition(Alignment.Right), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex - 1))
                        _dungeons[index.MiddleIndex - 1]
                                .transform
                                .DOMove(GetPosition(Alignment.Left), Duration);

                    if (_dungeons.ContainsKey(index.MiddleIndex + 2))
                        _dungeons[index.MiddleIndex + 2]
                                .transform
                                .DOMove(GetPosition(Alignment.RightInvisible), Duration);
                }
            }
        }

        private Vector3 GetPosition(Alignment alignment) =>
                Hierarchy.AlligmentHelper.GetPosition(alignment);
    }
}