using System;
using System.Collections;
using Helpers.Position;
using Units;
using UnityEngine;

namespace Realization.VisualEffects
{
    public class VisualEffect : MonoBehaviour
    {
        [SerializeField] private float _scaleMultiplier = 1.5f;
        [SerializeField] private float _destroyTime = 5;

        private bool _dontDestroy;
        private IMinion _target;
        public bool Destroed;

        private void Awake()
        {
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            yield return new WaitForSeconds(_destroyTime);
            
            if (_dontDestroy || Destroed)
                yield break;
            DestroyEffect();
        }

        public void Place(Vector2 worldPosition)
        {
            transform.position = worldPosition;
        }

        public void Scale(int radius)
        {
            transform.localScale *= radius * _scaleMultiplier;
        }

        public void DontDestroy()
        {
            _dontDestroy = true;
        }

        public void DestroyEffect()
        {
            if(Destroed)
                return;
            Destroed = true;
            if(this == null || this.gameObject == null)
                return;
            Destroy(gameObject);
        }

        public void SetParent(IMinion target)
        {
            gameObject.transform.SetParent(target.GameObject.transform);
            _target = target;
            _target.Destroying += DestroyEffect;
        }

        private void DestroyEffect(IDestroyablePoint obj)
        {
            _target.Destroying -= DestroyEffect;
            DestroyEffect();
        }

        public void Invert()
        {
            var inverted = transform.localScale;
            inverted.x = -inverted.x;
            transform.localScale = inverted;
        }
    }
}