using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Helpers
{
    public class Follower : MonoBehaviour, IDisposable
    {
        private List<Image> _images;
        private List<TMP_Text> _texts;
        private List<SpriteRenderer> _renderers;

        private Transform _target;
        private Vector3 _offset = new Vector3(50, -50, 0);
        private Sequence _followBetween;
        private Vector3 _additionalOffset;

        public void Init(Transform target, Vector2 additionalOffset)
        {
            _additionalOffset = additionalOffset;
            _target = target;
            _followBetween.Kill();

            _images = new(gameObject.GetComponentsInChildren<Image>());
            _texts = new(gameObject.GetComponentsInChildren<TMP_Text>());
            _renderers = new(gameObject.GetComponentsInChildren<SpriteRenderer>());
        }

        public void Init(List<Transform> targets)
        {
            _additionalOffset = Vector3.zero;
            _target = null;
            
            var offset = new Vector3();
            if (transform.parent.name == "Canvas_HardTutorial_another")
            {
                offset = new Vector3(0.5f, 0.2f);
            }else
                offset = new Vector3(50f, 20f);
            
            var sequence = CreateMoving(targets, offset);
        }

        private Sequence CreateMoving(List<Transform> targets, Vector3 offset)
        {
            _followBetween = DOTween.Sequence();
            transform.position = targets[0].position+offset;
            
            _followBetween.Append(transform.DOMove(targets[0].position + offset, 
                0.35f));
            for (var index = 1; index < targets.Count; index++)
            {
                var target = targets[index];
                _followBetween.Append(transform.DOMove(target.position + offset, 
                    0.2f*(Vector2.Distance(target.position, targets[index-1].position))));
            }
            _followBetween.Append(transform.DOMove(targets[^1].position + offset, 
                0.35f));
            
            _followBetween.OnComplete((() =>
            {
                CreateMoving(targets, offset);
            }));
            
            _followBetween.Play();
            return _followBetween;
        }

        private void Update()
        {
            if (_target == null)
            {
                // Destroy(gameObject);
                return;
            }

            SyncRenderer();
            if (transform.parent.name == "Canvas_HardTutorial_another")
            {
                transform.position = _target.position+new Vector3(0.5f, 0.5f)+(_additionalOffset);
            }else
                transform.position = _target.position+_offset+(_additionalOffset*100);
        }

        private void SyncRenderer()
        {
            bool enabled = _target.gameObject.activeInHierarchy;

            foreach (Image image in _images)
            {
                image.enabled = enabled;
            }

            foreach (TMP_Text text in _texts)
            {
                text.enabled = enabled;
            }

            foreach (SpriteRenderer renderer in _renderers)
            {
                renderer.enabled = enabled;
            }
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}