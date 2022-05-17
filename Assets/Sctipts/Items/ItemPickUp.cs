using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;

    public override void Interact(GameObject item, Transform player)
    {
        base.Interact(item, player);

        if (isInteractable)
            PickUp();
    }

    private void PickUp()
    {
        var res = InventoryController.instance.AddItem(item);
        
        if (res)
        {
            Debug.Log($"Picked up {item.name}.");
            Destroy(this.gameObject);
        }
    }
}
