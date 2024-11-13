using UnityEngine;

namespace Realization.Utils
{
    public class ConsoleToGUI : MonoBehaviour
    {
        private static readonly Vector2 Size = new(420, 185);
        private static readonly Vector2 PositionOffset = new(600, 600);
        private static readonly Vector2 TextPositionOffset = new(10, 200);

        [SerializeField] private bool _doShow = true;

        private string _myLog = "*begin log";

        private void Awake()
        {
            Application.logMessageReceived += Log;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Log(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Exception && type != LogType.Error)
                return;

            _myLog = _myLog + "\n" + logString + "\n" + stackTrace;
        }

        void OnGUI()
        {
            if (!_doShow)
            {
                return;
            }

            GUI.matrix = Matrix4x4.TRS(
                new Vector3(Screen.width - PositionOffset.x, Screen.height - PositionOffset.y),
                Quaternion.identity,
                new Vector3(
                    Screen.width / ScreenUtils.DefaultWidth,
                    Screen.height / ScreenUtils.DefaultHeight,
                    1.0f));

            GUI.skin.textArea.fontSize = 12;
            GUI.TextArea(new Rect(TextPositionOffset.x, TextPositionOffset.y, Size.x, Size.y), _myLog);
        }
    }
}