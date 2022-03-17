using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Brackeys:
    public static InventoryManager Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<InventoryManager>();
                if(current == null)
                {
                    Debug.LogError("Could not find an InventoryManager. Did you forget to add one to your scene?");
                }
            }
            return current;
        }
    }

    private InventorySlot[] allInventorySlots = null;
    private static InventoryManager current = null;

    public void Start()
    {
        FindAllInventorySlots();
    }

    private void FindAllInventorySlots()
    {
        allInventorySlots = FindObjectsOfType<InventorySlot>();
    }
}