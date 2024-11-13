using UnityEngine;
using Zenject;

namespace Realization.Installers.Abstract
{
    public abstract class FactoryInstaller<TProduct, TDerivedFactory, TContractFactory> : MonoInstaller, IInitializable
        where TDerivedFactory : TContractFactory
        where TContractFactory : IInstallableFactory<TProduct>
    {
        [SerializeField] private Object _prefab;

        private void OnValidate()
        {
            if ((_prefab is GameObject) == false
                || ((GameObject) _prefab).TryGetComponent(out TProduct getableInterface) == false)
            {
                Debug.LogWarning($"Wrong {nameof(_prefab)} " +
                                 $"in {GetType().Name} of {gameObject.name}." +
                                 $" It will be removed.");
                _prefab = null;
            }
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo(GetType())
                .FromInstance(this)
                .AsSingle();

            Container.Bind<TContractFactory>()
                .To<TDerivedFactory>()
                .AsSingle();
        }

        public void Initialize()
        {
            TContractFactory factory = Container.Resolve<TContractFactory>();
            factory.Load(_prefab);
        }
    }
}