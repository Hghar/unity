using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.UI
{
    public class ResetButton : MonoBehaviour
    {
        private Button _button;

        public event Action Showed;

        private void Start()
        {
            (_button ??= GetComponent<Button>()).onClick.AddListener(ShowAd);
        }

        private void ShowAd()
        {
            return;
            if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
            {
                Appodeal.Show(AppodealShowStyle.RewardedVideo);
                Showed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}