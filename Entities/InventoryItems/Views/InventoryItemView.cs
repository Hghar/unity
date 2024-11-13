using Model.Inventories.Items;
using TMPro;
using UI.DragAndDrop.InventoryItem;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.InventoryItems.Views
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private DraggableSlot _draggable;

        public void Rename(string newName)
        {
            if (_name != null)
                _name.text = newName;
        }

        public void InitDraggable(IInventoryItem item)
        {
            _draggable.Item = item;
        }

        public void ChangeSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }
}