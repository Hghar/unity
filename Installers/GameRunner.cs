using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installers
{
    public class GameRunner : MonoBehaviour
    {
        GameBootstrapper.Factory _gameBootstrapperFactory;

        [Inject]
        void Construct(GameBootstrapper.Factory bootstrapperFactory) => 
                _gameBootstrapperFactory = bootstrapperFactory;

        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();
      
            if(bootstrapper != null) return;

            _gameBootstrapperFactory.Create();
        }
    }
}