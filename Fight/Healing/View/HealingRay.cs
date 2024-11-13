using System.Collections;
using Helpers.Position;
using UnityEngine;

namespace Fight.Healing.View
{
    public class HealingRay : MonoBehaviour
    {
        [SerializeField] private Transform _walker;
        [SerializeField] private float _speed;

        private IReadOnlyPosition _toPoint;
        private Transform _fromPoint;
        private Coroutine _showing;
        private bool _isShowing;

        public bool IsShowing => _isShowing;

        public void ShowRay(Transform fromPoint, IReadOnlyPosition toPoint)
        {
            _fromPoint = fromPoint;
            _toPoint = toPoint;

            _showing = StartCoroutine(ShowingRay());
            _walker.gameObject.SetActive(true);
            _isShowing = true;
        }

        public void StopShowing()
        {
            _walker.gameObject.SetActive(false);

            if (_showing != null)
            {
                StopCoroutine(_showing);
                _showing = null;
            }

            _isShowing = false;
        }

        private IEnumerator ShowingRay()
        {
            bool isGoingToTarget = true;

            while (_toPoint != null && _fromPoint != null)
            {
                Vector2 targetPoint = isGoingToTarget ? _toPoint.WorldPosition : _fromPoint.position;
                _walker.position = Vector2.MoveTowards(_walker.position, targetPoint, _speed * Time.deltaTime);

                if ((Vector2) _walker.position == targetPoint)
                    isGoingToTarget = !isGoingToTarget;

                yield return null;
            }
        }
    }
}