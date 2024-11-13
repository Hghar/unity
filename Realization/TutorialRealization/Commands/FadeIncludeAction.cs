using Cysharp.Threading.Tasks;
using Infrastructure.Services.StaticData;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class FadeIncludeAction : IAction
    {
        private readonly HardTutorial _hardTutorial;
        private readonly IObjectProvider<GameObject> _excluded;

        public FadeIncludeAction(HardTutorial hardTutorial, IObjectProvider<GameObject> excluded)
        {
            _excluded = excluded;
            _hardTutorial = hardTutorial;
        }

        public async UniTask Perform()
        {
            GameObject gameObject = await _excluded.GetAsync();
            _hardTutorial.IncludeInFade(gameObject);
        }
    }
}