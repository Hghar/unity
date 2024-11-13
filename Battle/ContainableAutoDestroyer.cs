using System.Collections;
using UnityEngine;

namespace Battle
{
    public class ContainableAutoDestroyer : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => transform.childCount == 0);
            Destroy(gameObject);
        }
    }
}