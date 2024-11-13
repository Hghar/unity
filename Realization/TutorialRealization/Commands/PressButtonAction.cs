using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Commands
{
    public class PressButtonAction : IAction
    {
        private IObjectProvider<Button> _buttonToPress;

        public PressButtonAction(IObjectProvider<Button> buttonToPress)
        {
            _buttonToPress = buttonToPress;
        }

        public async UniTask Perform()
        {
            var button = await _buttonToPress.GetAsync();
            button.onClick.Invoke();
        }
    }
}