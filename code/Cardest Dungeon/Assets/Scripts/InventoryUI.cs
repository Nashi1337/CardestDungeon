using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Adds an item to the graphical User Interface
    /// </summary>
    /// <param name="item">the item to add</param>
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
    /// <param name="destroy">should the Gameobject be deleted or not</param>
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
