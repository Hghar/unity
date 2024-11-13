using Entities.Factories;
using Entities.Tile.Views;
using Infrastructure.CompositeDirector;
using Infrastructure.Shared.Factories;
using Model.Maps;

namespace Entities.Tile
{
    public class TileFactory : EntityFactory<PlayableTileEntity, TileFactoryArgs>
    {
        private readonly IFactory<IPlayableTileView, TileFactoryArgs> _viewFactory;
        private readonly IMap _map;

        public TileFactory(CompositeDirector director, IFactory<IPlayableTileView, TileFactoryArgs> viewFactory,
            IMap map) : base(director)
        {
            _viewFactory = viewFactory;
            _map = map;
        }

        protected override PlayableTileEntity CreateInternal(TileFactoryArgs args)
        {
            IPlayableTileView view = _viewFactory.Create(args);
            return new PlayableTileEntity(view, args.Tile, _map);
        }
    }
}