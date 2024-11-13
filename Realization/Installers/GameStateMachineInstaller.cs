using System;
using System.Linq;
using Fight.Fractions;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using Realization.General;
using Realization.UI;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class GameStateMachineInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UnitFactory _unitFactory;// Todo: Move bind  UnitFactory to GameInstaller 

        private MinionFactory _minionFactory;
        private IMinion _minion;

        public override void InstallBindings()
        {
            Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();
            Container.BindInterfacesTo<GameStateMachineInstaller>().FromInstance(this).AsSingle();

        }

        private void OnUnitCreated(IMinion minion)
        {
            minion.Died += LoseGame;
        }

        private void OnDestroy()
        {

        }

        private void LoseGame(IMinion minion)
        {
            minion.Died -= LoseGame;
           // _minionFactory.Created -= OnUnitCreated;
            if (minion.Fraction == Fraction.Enemies) return;
            if (_minionFactory.Minions.Where((minion1 => minion1.Fraction == Fraction.Minions)).Any())
                return;
            
            var gameStateMachine = Container.Resolve<IGameStateMachine>();
            gameStateMachine.Enter<LoseState>();
        }

      
        public void Initialize()
        {
            _minionFactory = Container.Resolve<MinionFactory>();

            /*var gameStateMachine = Container.Resolve<IGameStateMachine>();
            gameStateMachine.Enter<BootstrapState>();
            */

            _minionFactory.Created += OnUnitCreated;
        }
    }
}