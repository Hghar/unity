using UnityEngine;

namespace Fight // TODO: locate it to the view directory
{
    public class AfterDeathInstantiator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _container;

        private void OnDestroy()
        {
            Instantiate(_prefab, transform.position, _prefab.transform.rotation, _container);
        }
    }
}