using System;
using System.Collections.Generic;
using Entities.Tile.Views;
using Model.Maps.Types;
using UnityEngine;
using Zenject;

namespace Entities.Tile
{
    public class TileViewFactory : Infrastructure.Shared.Factories.IFactory<IPlayableTileView, TileFactoryArgs>
    {
        private readonly DiContainer _container;
        private readonly Dictionary<Type, PlayableTileView[]> _prefabs;
        private readonly Dictionary<Type, int> _iterators = new();
        private Transform _parent;

        public TileViewFactory(DiContainer container, Dictionary<Type, PlayableTileView[]> prefabs, Transform parent)
        {
            _container = container;
            _parent = parent;
            _prefabs = prefabs;
            foreach (KeyValuePair<Type, PlayableTileView[]> prefab in _prefabs)
            {
                _iterators.Add(prefab.Key, 0);
            }
        }

        public IPlayableTileView Create(TileFactoryArgs args)
        {
            PlayableTileView[] prefabs = _prefabs[args.TileType];
            int iterator = _iterators[args.TileType];

            IPlayableTileView instance =
                _container.InstantiatePrefab(prefabs[iterator], _parent).GetComponent<IPlayableTileView>();
            _iterators[args.TileType] = ++iterator % prefabs.Length;
            return instance;
        }
    }

    public readonly struct TileFactoryArgs
    {
        public readonly ITile Tile;
        public Type TileType => Tile.GetType();

        public TileFactoryArgs(ITile tile)
        {
            Tile = tile;
        }
    }
}