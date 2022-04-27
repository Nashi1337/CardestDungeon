using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
/*    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged(Equipment newItem *//*, Equipment oldItem *//*);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;*/

/*    void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }*/

/*    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex);

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem *//*, oldItem *//*);
        }

        currentEquipment[slotIndex] = newItem;
    }*/

/*    public Equipment Unequip(int slotIndex)
    {
        Equipment oldItem = null;

        if(currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

        }
            return oldItem;

        
    }*/
}
