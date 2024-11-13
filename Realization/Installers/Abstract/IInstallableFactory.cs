using UnityEngine;

namespace Realization.Installers.Abstract
{
    public interface IInstallableFactory<TProduct>
    {
        public void Load(Object prefab);
        public TProduct Create(Vector3 position = new Vector3(), Transform container = null);
    }
}