using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class HardIncludeAction : IAction
    {
        private readonly HardTutorial _hardTutorial;
        private readonly IObjectProvider<GameObject> _included;

        public HardIncludeAction(HardTutorial hardTutorial, IObjectProvider<GameObject> included)
        {
            _included = included;
            _hardTutorial = hardTutorial;
        }

        public async UniTask Perform()
        {
            GameObject gameObject = await _included.GetAsync();
            Debug.Log($"Including {gameObject?.name}");
            _hardTutorial.Include(gameObject);
        }
    }
}