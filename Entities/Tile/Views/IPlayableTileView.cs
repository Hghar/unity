using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Tile.Views
{
    public interface IPlayableTileView : IDisposable
    {
        event PressMoveButton MoveButtonPressed;

        void MoveTo(Vector2Int position);
        void Rename(string newName);
        void ExitedTo(Vector2 direction);
        void DeactivateButtonsWithoutWalls();
        void OpenOnMap();
        void Enter();
        void SetBlockedTiles(Vector2[] directions);
        void Disable();
        void Enable();
        void OpenDoors(List<Vector2> blocked);
        void AnimateTopDoor();
    }
}