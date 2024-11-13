using UnityEngine;
using Zenject;

namespace Realization.Installers.Abstract
{
    public abstract class SingletonComponentInstaller<TDerived, TContract> : MonoInstaller
        where TDerived : MonoBehaviour, TContract
    {
        [SerializeField] private TDerived _installable;

        private void OnValidate()
        {
            if (_installable == null)
                _installable = FindObjectOfType<TDerived>();
        }

        public override void InstallBindings()
        {
            Container.Bind<TContract>().FromInstance(_installable).AsSingle();
        }
    }
}