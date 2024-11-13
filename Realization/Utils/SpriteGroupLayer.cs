using NaughtyAttributes;
using UnityEngine;

namespace Realization.Utils
{
    public class SpriteGroupLayer : MonoBehaviour
    {
        [SortingLayer] [SerializeField] private string _layer;
        [SerializeField] private SpriteMaskInteraction _interaction;

        private void Awake()
        {
            SpriteRenderer[] children = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer child in children)
            {
                child.sortingLayerName = _layer;
                child.maskInteraction = _interaction;
            }
        }
    }
}