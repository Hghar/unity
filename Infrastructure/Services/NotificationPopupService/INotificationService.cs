namespace Infrastructure.Services.NotificationPopupService
{
    public interface INotificationService
    {
        void CreateRoot();
        void Close(PopUpId popUpId);
        void SetDefaultSitting(NotificationService.Sitting sitting);
        IPopup Show(string text,NotificationService.Sitting sitting);
        IPopup Show(string text);
    }

    public interface IPopup
    {
        void DestroyPopup();
    }

    public enum PopUpId
    {
        Simple = 1,
        Info = 2,
    }

    
}