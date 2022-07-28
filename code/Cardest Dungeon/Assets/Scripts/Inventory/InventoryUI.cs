using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the inventory UI in the dungeon. It is seperate from the actual inventory and therefore needs to updated
/// appropriatly if the state of the player inventory changes.
/// This class is not even close to be in a finished state.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    /// <summary>
    /// Initializes the InventoryUI. This method needs to be called before the inventoryUI is used.
    /// </summary>
    public void Initialize()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }

    private void Update()
    {
        if (InputManager.GetActionDown(InputManager.inventory))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    private void UpdateUI()
    {

        //Debug.Log("Updating UI");

        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
