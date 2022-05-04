using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<InventoryManager>();
                if(current == null)
                {
                    //Die Meldung kommt ständig weil kein InventoryManager in der Szene ist... aber man braucht keinen? weird
                    //Debug.LogError("Could not find an InventoryManager. Did you forget to add one to your scene?");
                }
            }
            return current;
        }
    }

    private InventorySlot[] allInventorySlots = null;
    private static InventoryManager current = null;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Awake if");
        }
        else
        {
            Debug.Log("Awake else");
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