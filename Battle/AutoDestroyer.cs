using UnityEngine;

namespace Battle
{
    public class AutoDestroyer : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}