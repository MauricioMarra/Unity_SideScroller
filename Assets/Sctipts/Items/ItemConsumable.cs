using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item consumable", menuName = "Inventory/Item Consumable")]
public class ItemConsumable : Item
{
    public int value;
    public ConsumableType consumableType;
}

public enum ConsumableType
{
    health,
    mana
}
