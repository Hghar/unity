using System.Collections.Generic;
using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UiAttachPointsInstaller : MonoInstaller
    {
        [SerializeField] private InventoryPoint _inventory;

        public override void InstallBindings()
        {
            List<IConnectPoint> points = new List<IConnectPoint> {_inventory};

            Container.Bind<IConnectPointList>().FromInstance(new ConnectPointList(points.ToArray())).AsSingle();
        }
    }
}