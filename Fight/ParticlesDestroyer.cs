using UnityEngine;

namespace Fight
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesDestroyer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            Destroy(gameObject, _particleSystem.main.duration);
        }
    }
}