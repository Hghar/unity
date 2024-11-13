using UnityEngine;
using Zenject;

namespace Units.Picking
{
    public class ExistingPickableUnit : MonoBehaviour
    {
        [SerializeField] private PickableUnit _pickableUnit;

        [Inject]
        private void Construct(IPickableUnitPool pickableUnitPool)
        {
            pickableUnitPool.TryAdd(_pickableUnit);
        }

        private void OnEnable()
        {

            Debug.Log(this.transform.gameObject.name);
        }
    }
}