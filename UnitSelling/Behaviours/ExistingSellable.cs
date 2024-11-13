using UnitSelling.Picking;
using UnityEngine;
using Zenject;

namespace UnitSelling.Behaviours
{
    public class ExistingSellable : MonoBehaviour
    {
        [SerializeField] private SellableBehaviour _sellableBehaviour;

        [Inject]
        private void Construct(ISellablePool pool)
        {
            pool.TryAdd(_sellableBehaviour.Sellable);
        }
    }
}