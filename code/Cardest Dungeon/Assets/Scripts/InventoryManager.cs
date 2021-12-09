using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryManager Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<InventoryManager>();
            }
            return current;
        }
    }

    [SerializeField]
    private GameObject itemIconTemplate;
    private InventoryManager current = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the GUI to show all items from the given item list.
    /// </summary>
    /// <param name="itemlist">All items that are supposed to be drawn on the GUI from left to right.</param>
    public void UpdateGUI(Item[] itemlist)
    {
        throw new System.Exception("Method not implemented");
    }
}
