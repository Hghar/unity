using Cysharp.Threading.Tasks;
using Infrastructure.Services.NotificationPopupService;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;

namespace Realization.TutorialRealization.Commands
{
    public class HintCloseAction : IAction
    {
        private PopUpId _id;
        private INotificationService _notificationService;

        public HintCloseAction(PopUpId id, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _id = id;
        }

        public UniTask Perform()
        {
            _notificationService.Close(_id);
            return UniTask.CompletedTask;
        }
    }
}