using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    new public string name;
    public ItemType itemType;
    public Sprite Icon = null;
    public int price;

    public virtual void Use()
    {
        Debug.Log($"Used {this.name}");
    }

}

public enum ItemType
{
    potion,
    money
}