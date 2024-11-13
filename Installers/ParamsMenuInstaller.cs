using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ParamsMenuInstaller : MonoInstaller
    {
        [SerializeField] private ParamsMenu _paramsMenu;
        [SerializeField] private InfoHint _infoHint;

        private void OnValidate()
        {
            if (_paramsMenu == null)
                _paramsMenu = FindObjectOfType<ParamsMenu>();
        }

        public override void InstallBindings()
        {
            Container.Bind<IParamsMenu>().FromInstance(_paramsMenu).AsSingle();
            Container.Bind<InfoHint>().FromInstance(_infoHint).AsSingle();
        }
    }
}