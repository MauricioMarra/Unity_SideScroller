using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tradable : MonoBehaviour
{
    [SerializeField] Item item;
    InventoryController inventory;
    private PlayerInputAction input;
    private bool isInteratable = false;

    private void Awake()
    {
        input = new PlayerInputAction();
    }

    private void Start()
    {
        inventory = InventoryController.instance;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (input.Character.PlayerInteraction.triggered && isInteratable)
            Buy();
    }

    public void Buy()
    {
        if (inventory.GetPurse() >= item.price)
        {
            inventory.AddItem(item);
            inventory.DecreasePurse(item.price);
        }
        else
            Debug.Log($"Not enough money to buy {item.name}");
    }

    public void Sell()
    {

    }

    //Before I used to get the key inside OnTriggerStay2D and it caused too much lag
    //to catch the key pressed.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInteratable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInteratable = false;
    }
}
