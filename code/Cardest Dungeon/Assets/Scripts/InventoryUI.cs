using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the inventory UI in the dungeon. It is seperate from the actual inventory and therefore needs to updated
/// appropriatly if the state of the player inventory changes.
/// This class is not even close to be in a finished state.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<InventoryUI>();
            }
            return current;
        }
    }

    [SerializeField]
    //private float itemDelta; //Delta between two items
    private static InventoryUI current = null;

    /// <summary>
    /// Adds an item to the graphical User Interface
    /// </summary>
    /// <param name="item">The item to add</param>
    public void AddItem(Item item)
    {
        item.transform.SetParent(transform);

        float inventoryWidth = GetComponent<Image>().sprite.rect.width;
        float itemWidth = item.GetComponent<Image>().sprite.rect.width;
        float offset = + 0.5f * itemWidth - 0.5f * inventoryWidth;

        //item.transform.localPosition = new Vector3(itemDelta * (transform.childCount - 1) + offset, 0, 0);
    }

    /// <summary>
    /// Removes an item from the graphical User Interface
    /// </summary>
    /// <param name="item">the item to remove</param>
    public void RemoveItem(Item item)
    {
        GameObject search = null;

        foreach (Transform i in item.transform)
        {
            if (item.gameObject.Equals(i.gameObject))
            {
                search = null;
                break;
            }
        }
        Destroy(search);
    }
}
