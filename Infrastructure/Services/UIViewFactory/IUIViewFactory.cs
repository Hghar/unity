using Infrastructure.Services.WindowService.MVVM;

namespace Infrastructure.Services.UIViewFactory
{
    public interface IUIViewFactory
    {
        void CreateUIRoot();
        MenuView CreateMenuWindowView();
        LoseScreenView CreateLoseScreen();
		LevelingView CreateLevelingWindowView();
        CoreSittingsView CreateCoreSittingsView();
        WinScreenView CreateWinScreenView();
    }
}