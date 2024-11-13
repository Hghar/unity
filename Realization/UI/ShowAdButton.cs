using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.UI
{
    public class ShowAdButton : MonoBehaviour
    {
        [SerializeField] private bool _rewarded;

        private Button _button;

        private void Start()
        {
            PlayerPrefs.SetInt("AdsWatched", 0);
            (_button ??= GetComponent<Button>()).onClick.AddListener(ShowAd);
            if (_rewarded && Appodeal.IsLoaded(AppodealAdType.RewardedVideo) == false)
                _button.interactable = false;
        }

        private void ShowAd()
        {
            if (_rewarded)
            {
                PlayerPrefs.SetInt("AdsWatched", 1);
                if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
                {
                    Appodeal.Show(AppodealShowStyle.RewardedVideo);
                }
            }
            else if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
            {
                Appodeal.Show(AppodealShowStyle.Interstitial);
            }
        }
    }
}