using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    /*    public void OnHandlePickupItem()
        {
            InventorySystem.current.Add(referenceItem);
            Destroy(gameObject);
        }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            InventorySystem.current.Add(referenceItem);
            Destroy(gameObject);
        }
    }
}
