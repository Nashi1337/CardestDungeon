using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager current = null;
    private InventorySlot[] allInventorySlots = null;
    public static InventoryManager Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<InventoryManager>();
                if(current == null)
                {

                }
            }
            return current;
        }
    }


    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Start()
    {
        FindAllInventorySlots();
    }

    private void FindAllInventorySlots()
    {
        allInventorySlots = FindObjectsOfType<InventorySlot>();
    }
}