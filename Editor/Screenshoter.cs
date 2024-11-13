using NaughtyAttributes;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = nameof(Screenshoter), menuName = nameof(Screenshoter), order = 0)]
    public class Screenshoter : ScriptableObject
    {
        [SerializeField] private string _path;

        private int _name = 0;

        [Button]
        public void MakeSkreenshot()
        {
            ScreenCapture.CaptureScreenshot(_path + "\\" + _name.ToString() + ".png", 1);
            _name++;
        }
    }
}