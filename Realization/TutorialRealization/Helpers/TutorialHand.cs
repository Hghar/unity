using System;
using System.Collections.Generic;
using DG.Tweening;
using Realization.TutorialRealization.Commands;
using UnityEngine;

namespace Realization.TutorialRealization.Helpers
{
    public class TutorialHand
    {
        private Transform _handWorld;
        private Transform _handCamera;
        private Transform _handOverlay;

        private Transform _currentHand;

        public TutorialHand(Transform handWorld, Transform handCamera, Transform handOverlay)
        {
            _handOverlay = handOverlay;
            _handCamera = handCamera;
            _handWorld = handWorld;
        }

        private Transform Get(RenderSpace renderSpace)
        {
            switch (renderSpace)
            {
                case RenderSpace.World:
                    DeactivateHandsExcept(_handWorld);
                    return _handWorld;
                case RenderSpace.CanvasCamera:
                    DeactivateHandsExcept(_handCamera);
                    return _handCamera;
                case RenderSpace.CanvasOverlay:
                    DeactivateHandsExcept(_handOverlay);
                    return _handOverlay;
                default:
                    throw new ArgumentOutOfRangeException(nameof(renderSpace), renderSpace, null);
            }
        }

        private void DeactivateHandsExcept(Transform excepted)
        {
            _handWorld.gameObject.SetActive(false);
            _handCamera.gameObject.SetActive(false);
            _handOverlay.gameObject.SetActive(false);

            excepted.gameObject.SetActive(true);
        }

        public void Follow(RenderSpace type, Transform target, bool flip, float rotation, Vector2 offset)
        {
            _currentHand = Get(type);
            
            var scale = new Vector3(1, 1, 1);
            if (flip)
            {
                scale.x = -1;
                // scale.y = -1;
            }
            _currentHand.transform.localScale = scale;
            _currentHand.rotation = Quaternion.Euler(0, 0, rotation);
            

            if (_currentHand.gameObject.TryGetComponent(out Follower follower))
            {
                follower.Init(target, offset);
                AnimateHand(_currentHand.gameObject);
                return;
            }

            _currentHand.gameObject.AddComponent<Follower>().Init(target, offset);
            AnimateHand(_currentHand.gameObject);
        }

        public void Follow(RenderSpace type, List<Transform> targets)
        {
            _currentHand = Get(type);

            if (_currentHand.gameObject.TryGetComponent(out Follower follower))
            {
                follower.Init(targets);
                return;
            }

            _currentHand.gameObject.AddComponent<Follower>().Init(targets);
            StopAnimation(_currentHand.gameObject);
        }

        public void Disable()
        {
            _handOverlay.gameObject.SetActive(false);
            _handCamera.gameObject.SetActive(false);
            _handWorld.gameObject.SetActive(false);
            
            StopAnimation(_handOverlay.gameObject);
            StopAnimation(_handCamera.gameObject);
            StopAnimation(_handWorld.gameObject);
        }

        private void AnimateHand(GameObject hand)
        {
            hand.GetComponentInChildren<Animator>().SetBool("animate", true);
            // hand.transform.DOKill(true);
            // hand.transform.DOPunchScale(Vector3.one/10, 1).SetLoops(-1);
        }

        private void StopAnimation(GameObject hand)
        {
            hand.GetComponentInChildren<Animator>().SetBool("animate", false);
            // hand.transform.DOKill();
        }
    }
}