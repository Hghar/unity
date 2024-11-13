using Firebase.Analytics;
using TMPro;
using UnityEngine;

namespace Realization.UI
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text _info;

        private void Awake()
        {
            int index = PlayerPrefs.GetInt("level");
            switch (index)
            {
                case 0:
                    _info.text = $"Tutorial Level";
                    FirebaseAnalytics.LogEvent("tutorial_level_started");
                    break;
                case 1:
                    _info.text = $"Level 1\nEnemies: skeletons";
                    FirebaseAnalytics.LogEvent("level_1_level_started");
                    break;
                case 2:
                    _info.text = $"Level 2\nEnemies: skeletons, zombies";
                    FirebaseAnalytics.LogEvent("level_2_level_started");
                    break;
                case 3:
                    _info.text = $"Level 3\nEnemies: zombies, mummies";
                    FirebaseAnalytics.LogEvent("level_3_level_started");
                    break;
                case 4:
                    _info.text = $"Level 4\nEnemies: mummies, demons";
                    FirebaseAnalytics.LogEvent("level_4_level_started");
                    break;
                case 5:
                    _info.text = $"Level 5\nEnemies: guests, mummies";
                    FirebaseAnalytics.LogEvent("level_5_level_started");
                    break;
            }
        }
    }
}