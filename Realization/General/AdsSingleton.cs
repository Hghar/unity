using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;

namespace Realization.General
{
    public class AdsSingleton : MonoBehaviour
    {
        private static AdsSingleton Instance;

        private void Start()
        {
            if (Instance != null)
                Destroy(gameObject);

            int adTypes = AppodealAdType.Interstitial | AppodealAdType.RewardedVideo;
            string appKey = "82e9c79f5e7d7da58e6620c69e27815f8fc2cdd252db28ed";
            AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
            Appodeal.Initialize(appKey, adTypes);
            Appodeal.SetAutoCache(AppodealAdType.Interstitial | AppodealAdType.RewardedVideo, true);
            Instance = this;
        }

        public void OnInitializationFinished(object sender, SdkInitializedEventArgs e)
        {
            Debug.Log("Initialization Finished");
        }
    }
}