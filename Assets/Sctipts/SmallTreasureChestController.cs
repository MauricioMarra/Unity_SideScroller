using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTreasureChestController : MonoBehaviour
{
    [SerializeField] Item item;
    private InventoryController inventory;

    private void Start()
    {
        inventory = InventoryController.instance;
    }

    private void Open()
    {
        var animator = this.GetComponent<Animator>();
        animator.SetTrigger("open");
        inventory.IncreasePurse(item.price);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HitBox")
            Open();
    }

    public void DestroyChest()
    {
        Destroy(this.gameObject);
    }
}
