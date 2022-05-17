using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public List<Item> items = new List<Item>();
    public static InventoryController instance;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int limit = 18;
    [SerializeField] private int purse = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool AddItem(Item newItem)
    {
        if (newItem.itemType == ItemType.money)
        {
            purse += newItem.price;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();

            return true;
        }

        if (items.Count < limit)
        {
            items.Add(newItem);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        else
        {
            Debug.Log("Inventory Full!");
            return false;
        }

        return true;
    }

    public void RemoveItem(Item itemToRemove)
    {
        if (items.Count > 0)
        {
            items.Remove(itemToRemove);
        }
        else
        {
            Debug.Log("No items in inventory.");
            return;
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public int GetPurse()
    {
        return purse;
    }

    public void IncreasePurse(int value)
    {
        purse += value;

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void DecreasePurse(int value)
    {
        purse -= value;

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
