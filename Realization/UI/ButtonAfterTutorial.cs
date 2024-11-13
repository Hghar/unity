using UnityEngine;

namespace Realization.UI
{
    public class ButtonAfterTutorial : MonoBehaviour
    {
        [SerializeField] private GameObject[] _toDeactivate;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("level") != 1)
            {
                gameObject.SetActive(false);
                return;
            }

            foreach (GameObject deactivatingObject in _toDeactivate)
            {
                deactivatingObject.SetActive(false);
            }
        }
    }
}