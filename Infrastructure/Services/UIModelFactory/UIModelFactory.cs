using Infrastructure.Services.WindowService;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.ViewFactory;
using Zenject;

namespace Infrastructure.Services.UIModelFactory
{
    public class UIModelFactory : IUIModelFactory
    {
        private readonly IInstantiator _instantiator;

        public UIModelFactory(
                IInstantiator instantiator,IViewFactory viewFactory
        )
        {
            _instantiator = instantiator;
        }

        public LevelingViewModel CreateLevelingViewModel() =>
                _instantiator.Instantiate<LevelingViewModel>();

        public MenuViewModel CreateMenuViewModel() => 
                _instantiator.Instantiate<MenuViewModel>();
    }
}