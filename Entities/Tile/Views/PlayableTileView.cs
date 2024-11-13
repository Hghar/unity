using System;
using System.Collections.Generic;
using Realization.Configs;
using UnityEngine;
using Zenject;

namespace Entities.Tile.Views
{
    public class PlayableTileView : MonoBehaviour, IPlayableTileView
    {
        [SerializeField] private TileButton[] _buttons;
        [SerializeField] private GameObject _closedIconOnMap;
        [SerializeField] private GameObject _currentView;
        [SerializeField] private Vector2 _size;
        
        private Vector2[] _blockedDirections;

        public event PressMoveButton MoveButtonPressed;
        public event Action Entered;
        public event Action Exited;

        [Inject]
        private void Construct(MapConfig config)
        {
            _size = config.RoomSize;
        }

        private void Awake()
        {
            foreach (TileButton button in _buttons)
            {
                button.MoveButtonPressed += OnMoveButtonPressed;
            }
        }

        private void OnDestroy()
        {
            foreach (TileButton button in _buttons)
            {
                button.MoveButtonPressed -= OnMoveButtonPressed;
            }
        }

        public void MoveTo(Vector2Int position)
        {
            transform.position = position * _size;
        }

        public void Rename(string newName)
        {
            name = newName;
        }

        public void DeactivateAllButtons()
        {
            foreach (TileButton button in _buttons)
            {
                button.Deactivate();
            }
        }

        public void ActivateAllButtons()
        {
            DeactivateAllButtons();

            foreach (TileButton button in _buttons)
            {
                if (button.name.Contains("right"))
                    button.Activate();
            }
        }

        public void ExitedTo(Vector2 direction)
        {
            DeactivateAllButtons();

            foreach (TileButton button in _buttons)
            {
                if (button.Direction == direction)
                {
                    if(button.name.Contains("right"))
                        button.Activate();
                }
            }

            _currentView.SetActive(false);
            Exited?.Invoke();
        }

        public void DeactivateButtonsWithoutWalls()
        {
            foreach (TileButton button in _buttons)
            {
                button.Deactivate();
            }
        }

        public void OpenOnMap()
        {
            _closedIconOnMap.SetActive(false);
            _currentView.SetActive(true);
        }

        public void Enter()
        {
            DeactivateButtonsOnly(_blockedDirections);

            foreach (TileButton button in _buttons)
            {
                button.TurnOn();
            }

            Entered?.Invoke();
        }

        public void SetBlockedTiles(Vector2[] directions)
        {
            _blockedDirections = directions;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void OpenDoors(List<Vector2> blocked)
        {
            foreach (TileButton button in _buttons)
            {
                button.ShowDoor();
                foreach (Vector2 direction in blocked)
                {
                    if (button.Direction == direction)
                    {
                        button.HideDoor();
                        break;
                    }
                }
            }
        }

        public void AnimateTopDoor()
        {
            foreach (TileButton button in _buttons)
            {
                button.TryAnimate();
            }
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void DeactivateButtonsOnly(Vector2[] directions)
        {
            ActivateAllButtons();

            foreach (Vector2 blockedPosition in directions)
            {
                foreach (TileButton button in _buttons)
                {
                    if (button.Direction == blockedPosition)
                    {
                        button.Deactivate();
                    }
                }
            }
        }

        private void OnMoveButtonPressed(Vector2 direction)
        {
            MoveButtonPressed?.Invoke(direction);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}