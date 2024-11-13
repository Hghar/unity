using UnityEngine;
using UnityEngine.EventSystems;

namespace Realization.Cameras.Minimap
{
    public class FullMap : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Panel _mapPanel;

        public void OnPointerClick(PointerEventData eventData)
        {
            _mapPanel.Open();
        }
    }
}