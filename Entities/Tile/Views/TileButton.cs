using Realization.Location;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Entities.Tile.Views
{
    public class TileButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private GameObject _wall;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private DoorAnimator _animator;

        private bool _disabled = true;

        public event PressMoveButton MoveButtonPressed;

        public Vector2 Direction => _direction;

        // private static bool isNextLevel = false;

        private void OnEnable()
        {
            // if (isNextLevel == false)
            // {
            //     StartCoroutine(NextLevelTimer());
            // }
        }

        private IEnumerator NextLevelTimer()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForFixedUpdate();
            }

            // if (!isNextLevel && this.gameObject.activeSelf)
            // {
            //     isNextLevel = true;
            //
            //     // NextRoom();
            // }

            //yield return new WaitForSeconds(2f);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //NextRoom();
        }

        private void NextRoom()
        {
            if (_disabled)
                return;

            // GetComponent<Button>()?.onClick.Invoke();
            // TryAnimate();
            // MoveButtonPressed?.Invoke(_direction);

            // isNextLevel = false;

            Deactivate();
        }

        private void OnDestroy()
        {
            MoveButtonPressed = null;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            //StartCoroutine(NextLevelTimer());

            //NextRoom();
        }

        public void HideDoor()
        {
            _view.SetActive(false);
            _wall.SetActive(true);
        }

        public void ShowDoor()
        {
            _view.SetActive(true);
            _wall.SetActive(false);
        }

        public void TryAnimate()
        {
            if (_animator != null)
                _animator.Open();
        }

        public void TurnOn()
        {
            _disabled = false;
        }
    }
}