using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour
{
    public Image slot;

    private Item item;

    public void AddItemToSlot(Item item)
    {
        this.item = item;

        slot.sprite = item.Icon;
        slot.enabled = true;
    }

    public void RemoveItemFromSlot()
    {
        this.item = null;

        slot.sprite = null;
        slot.enabled = false;
    }

    public void UseItem()
    {
        item.Use();
        InventoryController.instance.RemoveItem(item);
    }
}
