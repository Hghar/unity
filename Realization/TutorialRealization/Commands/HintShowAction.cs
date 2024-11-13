using Cysharp.Threading.Tasks;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.WindowService.MVVM;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class HintShowAction : IAction
    {
        private string _text;
        private PopUpId _id;
        private Alignment _alignment;
        private bool _closeButton;
        private INotificationService _notificationService;
        private HardTutorial _hardTutorial;

        public HintShowAction(string text, PopUpId id, Alignment alignment, bool closeButton, 
            INotificationService notificationService, HardTutorial hardTutorial)
        {
            _hardTutorial = hardTutorial;
            _notificationService = notificationService;
            _closeButton = closeButton;
            _alignment = alignment;
            _id = id;
            _text = text;
        }

        public UniTask Perform()
        {
            var popup = _notificationService.Show(_text, new NotificationService.Sitting(_alignment, _id, _closeButton));
            _hardTutorial.ExcludeFromFade(((MonoBehaviour) popup).gameObject);
            return UniTask.CompletedTask;
        }
    }
}