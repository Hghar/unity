using Cysharp.Threading.Tasks;
using Grids;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.General;
using Realization.Installers;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.TutorialRealization.Commands
{
    public class SpawnUnitAction : IAction
    {
        private Character _character;
        private Vector2Int _position;

        public SpawnUnitAction(Character character, DiContainer container,
            Vector2Int position)
        {
            _position = position;
            _character = character;
        }

        public UniTask Perform()
        {
            if (_character.Tags.Contains("ally"))
            {
                var unit = ShopInstaller.MinionFactory.CreateAndReturn(_character);
                Object.FindObjectOfType<SceneContext>().Container
                    .Resolve<IGrid<IMinion>>().Place(unit, _position.x, _position.y);
                unit.UpdateWorldPosition(MoveType.Instantly);
            }
            else
            {
                var unit = Object.FindObjectOfType<EnemyFactory>().Create(_character);
                Object.FindObjectOfType<SceneContext>().Container
                    .Resolve<IGrid<IMinion>>().Place(unit, _position.x, _position.y);    
                unit.UpdateWorldPosition(MoveType.Instantly);
            }
            
            return UniTask.CompletedTask;
        }
    }
}