using Facebook.Unity;
using UnityEngine;

namespace Infrastructure.Services.Facebook
{
    public class FacebookInstance : MonoBehaviour
    {
        private static FacebookInstance _singleton;

        private void Awake()
        {
            if (_singleton == null)
            {
                _singleton = this;
                DontDestroyOnLoad(gameObject);
                
              //  Init();
            }
            else
            {
                Destroy(gameObject);
            }


        }

        public void Init()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback);
            }

            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }


        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }
    }
}