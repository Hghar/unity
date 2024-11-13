using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.Installers;
using Realization.Shops;
using Realization.States.CharacterSheet;
using Zenject;

namespace Realization.TutorialRealization.Commands
{
    public class SetShopAction : IAction
    {
        private int _shopIindex;
        private Character _character;
        private DiContainer _container;

        public SetShopAction(DiContainer container, Character character, int shopIindex)
        {
            _container = container;
            _character = character;
            _shopIindex = shopIindex;
        }

        public UniTask Perform()
        {
            var shop = ShopInstaller.Shop;
            shop.UpdateShopItem(_shopIindex, _character);
            return UniTask.CompletedTask;
        }
    }
}