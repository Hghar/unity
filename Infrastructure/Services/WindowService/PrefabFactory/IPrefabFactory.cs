using UnityEngine;

namespace Infrastructure.Services.WindowService.PrefabFactory
{
    public interface IPrefabFactory
    {
        T Instantiate<T>(string prefabName, Transform parent)
            where T : MonoBehaviour;
    }
}