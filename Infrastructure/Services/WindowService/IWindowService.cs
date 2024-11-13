namespace Infrastructure.Services.WindowService
{
    public interface IWindowService
    {
        void InitCore();
        void InitMeta();
        void ClearMeta();
        void ClearCore();
        void Open(WindowId windowId);
    }
}