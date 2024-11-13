using CompositeDirectorWithGeneratingComposites.CompositeDirector;
using CompositeDirectorWithGeneratingComposites.CompositeDirector.CompositeGeneration;
using Fight.Fractions;
using Model.Commands;
using NaughtyAttributes;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.DevTools
{
    public class EffectInvoker : MonoBehaviour
    {
        private CommandFacade _facade;

        [Inject]
        private void Construct(CommandFacade composite)
        {
            _facade = composite;
        }

        [Button()]
        private void IncreasePowerGladiators()
        {
            var command = _facade.MakeCommand("SetEffect_IncreasePower(7)_AllyGladiators_Instant");
            command.Perform();
        }

        [Button]
        private void DamageEnemy()
        {
            var command = _facade.MakeCommand("Active_IncreaseHealth(100)_AllAllies_Duration(5)");
            command.Perform();
        }
        
        [Button]
        private void HealEnemy()
        {
            var command = _facade.MakeCommand("Active_DecreaseHealth(100)_AllAllies_Duration(5)");
            command.Perform();
        }

        public void DoCommand(string commandText)
        {
            var command = _facade.MakeCommand(commandText);
            command.Perform();
        }
    }
}