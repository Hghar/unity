using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Tile.Views;
using Firebase.Analytics;
using Infrastructure.CompositeDirector.Executors;
using Model.Composites.Hidable;
using Model.Composites.Representation;
using Model.Maps;
using Model.Maps.Types;
using UnityEngine;

namespace Entities.Tile
{
    public class PlayableTileEntity : IRepresentation, IHidable
    {
        private readonly IPlayableTileView _view;
        private readonly ITile _tile;
        private readonly IMap _map;
        private bool _opened;

        public event Action<IProcessExecutor> Disposed;

        public PlayableTileEntity(IPlayableTileView view, ITile tile, IMap map)
        {
            _view = view;
            _tile = tile;
            _map = map;
            view.MoveButtonPressed += MoveCurrentTile;
            _view.Disable();
        }

        public void Represent()
        {
            if (_view.Equals(null) || _view == null)
                return;

            _view.MoveTo(_tile.Position);
            _view.Rename(_tile.GetType().Name);
            List<Vector2> blocked = _map.BlockedTilesAround(_tile).ToList();
            _view.SetBlockedTiles(blocked.ToArray());

            if (_map.Current != _tile)
            {
                Vector2 direction = _map.CurrentDirectionTo(_tile);
                _view.ExitedTo(direction);
                _opened = false;

                int x = Mathf.Abs((_map.Current.Position - _tile.Position).x);
                int y = Mathf.Abs((_map.Current.Position - _tile.Position).y);

                if ((x < 1 ||
                     y < 1) &&
                    Mathf.Abs(x - y) == 1
                   )
                {
                    _view.Enable();
                    _view.OpenDoors(blocked);
                    if (direction.y == 1)
                        _view.AnimateTopDoor();
                    return;
                }

                return;
            }

            _view.Enable();
            _view.OpenDoors(blocked);

            string message = string.Empty;
            if (_tile is BossTile)
            {
                message += "boss_";
            }
            else if (_tile is ShopTile)
            {
                ShopType shopType = (_view as PlayableTileView).GetComponent<ShopView>().ShopType;
                (_view as PlayableTileView).GetComponent<ShopView>().Clearing += ClearEnteties;
                if (shopType == ShopType.Characters)
                    message += "items_";
                else
                    message += "unknown_";

                message += "shop_";
            }
            else if (_tile is StartTile)
            {
                message += "start_";
            }
            else if (_tile is EmptyTile)
            {
                message += "empty_";
            }

            message += "room_visited_";
            message += "l" + PlayerPrefs.GetInt("level");

            if (_opened == false)
            {
                _opened = true;
                _view.OpenOnMap();
                FirebaseAnalytics.LogEvent(message, "is_first_time", 1);
                return;
            }

            FirebaseAnalytics.LogEvent(message, "is_first_time", 0);
            _view.Enter();
        }

        private void ClearEnteties()
        {
        }

        public void Hide()
        {
            _view.DeactivateButtonsWithoutWalls();
        }

        public void Dispose()
        {
            _view.MoveButtonPressed -= MoveCurrentTile;
            if (_view.Equals(null) == false)
                _view.Dispose();
            Disposed?.Invoke(this);
        }

        private void MoveCurrentTile(Vector2 direction)
        {
            _map.MoveTo(direction);
        }
    }
}