using Cysharp.Threading.Tasks;
using GameAnalyticsSDK.Setup;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.General;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class HardExcludeAction : IAction
    {
        private readonly HardTutorial _hardTutorial;
        private readonly IObjectProvider<GameObject> _excluded;

        public HardExcludeAction(HardTutorial hardTutorial, IObjectProvider<GameObject> excluded)
        {
            _excluded = excluded;
            _hardTutorial = hardTutorial;
        }

        public async UniTask Perform()
        {
            GameObject gameObject = await _excluded.GetAsync();
            _hardTutorial.Exclude(gameObject);
        }
    }
}