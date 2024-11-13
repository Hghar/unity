using Grids;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridBehaviour _behaviour;
        
        public override void InstallBindings()
        {
            var objectGrid = new ObjectGrid<IMinion>(new Vector2Int(8,4),_behaviour);
            
            Container.Bind<IGrid<IMinion>>().FromInstance(objectGrid);
        }
    }
}