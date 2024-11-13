using System;
using System.Collections;
using Fight;
using Pathfinding;
using UnityEngine;

namespace Units.Ai
{
    public class MinionAi : MonoBehaviour, IMinionAi
    {
        [SerializeField] private AILerp _aILerp; // TODO: use IAstarAI
        [SerializeField] private Mortality _mortality;

        public bool IsReachedDestination => _aILerp.reachedDestination;

        public event Action TookPosition;
        public event Action LeftPosition;
        public event Action<IMinionAi> Dying;
        public event Action<IMinionAi> Destroying;

        private void OnEnable()
        {
            _mortality.Dying += OnDying;
        }

        private void OnDisable()
        {
            _mortality.Dying -= OnDying;
        }

        private void OnDestroy()
        {
            Destroying?.Invoke(this);
        }

        private void Start()
        {
            if (_aILerp != null)
                StartCoroutine(WaitingForAiReachedDestination(() => OnAiReachedDestination()));
        }

        private void OnDying()
        {
            Dying?.Invoke(this);
        }

        private void OnAiReachedDestination()
        {
            TookPosition?.Invoke();
            StartCoroutine(WaitingForAiGotNewDestinaton(() => OnAiGotNewDestinaton()));
        }

        private void OnAiGotNewDestinaton()
        {
            LeftPosition?.Invoke();
            StartCoroutine(WaitingForAiReachedDestination(() => OnAiReachedDestination()));
        }

        private IEnumerator WaitingForAiReachedDestination(Action AiDestinationReached)
        {
            yield return new WaitUntil(() => _aILerp.reachedDestination);

            AiDestinationReached?.Invoke();
        }

        private IEnumerator WaitingForAiGotNewDestinaton(Action AiDestinationGotten)
        {
            yield return new WaitUntil(() => _aILerp.reachedDestination == false);
            AiDestinationGotten?.Invoke();
        }
    }
}