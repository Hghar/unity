using Infrastructure.Services.StaticData;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Realization.Economy
{
    public interface IHudFactory
    {
        HudFacade CreateHud();
    }

    public class HudFactory : IHudFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private const string HudPath = "Prefabs/Hud";

        public HudFactory(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _staticDataService = staticDataService;
        }

        public HudFacade CreateHud()
        {
            GameObject hudPrefab = Resources.Load<GameObject>(HudPath);
            GameObject hud = Object.Instantiate(hudPrefab);
            HudFacade hudFacade = hud.GetComponent<HudFacade>();
            
            _container.Inject(hudFacade.EconomyBehaviour);
            _container.Inject(hudFacade.OpenWindowButton);
            _container.Inject(hudFacade.CoinAnimation);
            
            return hudFacade;
        }
    }
}