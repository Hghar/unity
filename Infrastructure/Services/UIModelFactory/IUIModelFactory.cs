using Infrastructure.Services.WindowService.MVVM;

namespace Infrastructure.Services.UIModelFactory
{
    public interface IUIModelFactory
    {
        MenuViewModel CreateMenuViewModel();

        LevelingViewModel CreateLevelingViewModel();
    }
}