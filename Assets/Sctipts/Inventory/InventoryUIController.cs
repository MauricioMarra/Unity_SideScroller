using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    InventoryController inventory; //**obs

    public Transform itemsParent;
    private ItemSlotController[] slots;

    private PlayerInputAction inputAction;
    private bool visible = false;

    [SerializeField] Canvas uiCanvas;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventoryController.instance; //**obs -> Assim executa de forma cacheada. É mais performático.

        inventory.onItemChangedCallback += UpdateUI; //Subscribe para o delegate que criei no InventoryController.

        slots = itemsParent.GetComponentsInChildren<ItemSlotController>();

        inputAction.Inventory.OpenCloseInventory.performed += ToggleVisible;
    }

    void UpdateUI()
    {
        Debug.Log("Updating UI for inventory.");

        for(int i=0; i<slots.Length; i++)
        {
            if (i < inventory.items.Count)
                slots[i].AddItemToSlot(inventory.items[i]);
            else
                slots[i].RemoveItemFromSlot();
        }

        //foreach(var element in slots)
        //{
        //    for (int i = 0; i < inventory.items.Count; i++)
        //    {
        //        if (inventory.items[i] != null)
        //            slots[i].AddItemToSlot(inventory.items[i]);
        //        else
        //            slots[i].RemoveItemFromSlot();
        //    }
        //}

    }

    private void ToggleVisible(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        uiCanvas.enabled = !visible;
        visible = !visible;

        if (visible)
            GameManagerController._instance.PauseGame();
        else
            GameManagerController._instance.ResumeGame();
    }
}
