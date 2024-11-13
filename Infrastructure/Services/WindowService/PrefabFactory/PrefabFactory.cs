using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.WindowService.PrefabFactory
{
    internal sealed class PrefabFactory : IPrefabFactory
    {
        public T Instantiate<T>(string prefabName, Transform parent) where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(prefabName);

            if (prefab == null)
                throw new Exception($"Can't load '{typeof(T)}' by path '{prefabName}'");

            T instantiate = Object.Instantiate(prefab, parent);

            return instantiate;
        }
    }
}