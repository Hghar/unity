using Ticking;
using UnityEngine;
using Zenject;

public class GlobalUpdateTicker : MonoBehaviour
{
    private IGlobalTickable _tickablePool;

    [Inject]
    private void Construct(IGlobalTickable tickablePool)
    {
        _tickablePool = tickablePool;
    }

    private void Update()
    {
        _tickablePool.Tick(Time.deltaTime);
    }
}
