using NaughtyAttributes;
using UnityEngine;

namespace UI.Mock
{
    [RequireComponent(typeof(RectTransform))]
    public class RectSizeLogger : MonoBehaviour
    {
        [Button]
        private void LogRectSize()
        {
            Vector2 rectSize = GetComponent<RectTransform>().rect.size;
            Debug.Log(rectSize);
        }
    }
}