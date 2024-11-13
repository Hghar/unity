using UnityEngine;
using Zenject;

namespace Realization.Installers.Abstract
{
    public abstract class InstallableFactory<TProduct, TPool> : IInstallableFactory<TProduct>
        where TPool : IInstallablePool<TProduct>
    {
        private readonly IInstallableFactory<TProduct> _agregatedParent;
        private readonly TPool _pool;

        public InstallableFactory(DiContainer diContainer, TPool pool)
        {
            _agregatedParent = new AgregatedInstallableFactory<TProduct>(diContainer);
            _pool = pool;
        }

        public void Load(Object prefab)
        {
            _agregatedParent.Load(prefab);
        }

        public TProduct Create(Vector3 position, Transform container = null)
        {
            TProduct newProduct = _agregatedParent.Create(position, container);
            _pool.Add(newProduct);
            return newProduct;
        }

        private class AgregatedInstallableFactory<T> : InstallableFactory<T>
        {
            public AgregatedInstallableFactory(DiContainer diContainer) : base(diContainer)
            {
            }
        }
    }

    public abstract class InstallableFactory<TProduct> : IInstallableFactory<TProduct>
    {
        private readonly DiContainer _diContainer;

        private Object _prefab;

        public InstallableFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Load(Object prefab)
        {
            _prefab = prefab;
        }

        public TProduct Create(Vector3 position, Transform container = null)
        {
            return _diContainer.InstantiatePrefabForComponent<TProduct>(_prefab,
                position,
                Quaternion.identity,
                container);
        }
    }
}