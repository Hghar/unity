using System;
using UnityEngine;
using Zenject;

namespace Realization.TutorialRealization.Helpers
{
    public class TutorialCameraServiceInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;

        public override void InstallBindings()
        {
            Container.Bind<TutorialCameraService>().FromInstance(new TutorialCameraService(_camera)).AsSingle();
        }
    }
}