using UnityEngine;

namespace Realization.Cameras.Minimap
{
    public class Panel : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}