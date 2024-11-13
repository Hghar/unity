using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class FadeExcludeAction : IAction
    {
        private readonly HardTutorial _hardTutorial;
        private readonly IObjectProvider<GameObject> _excluded;

        public FadeExcludeAction(HardTutorial hardTutorial, IObjectProvider<GameObject> excluded)
        {
            _excluded = excluded;
            _hardTutorial = hardTutorial;
        }

        public async UniTask Perform()
        {
            GameObject gameObject = await _excluded.GetAsync();
            _hardTutorial.ExcludeFromFade(gameObject);
        }
    }
}