using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class FadeAction : IAction
    {
        private GameObject _fade;
        private bool _activate;
        private HardTutorial _hardTutorial;

        public FadeAction(GameObject fade, bool activate, HardTutorial hardTutorial)
        {
            _hardTutorial = hardTutorial;
            _activate = activate;
            _fade = fade;
        }

        public async UniTask Perform()
        {
            int t = 0;
            //todo fix
            if (_activate == false)
            {
                _hardTutorial.ClearFade();
            }
            
            while (t < 10)
            {
                _fade.SetActive(_activate);
                await Task.Yield();
                t++;
            }
        }
    }
}