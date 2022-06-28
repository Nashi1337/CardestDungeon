using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override bool Interact()
    {

        base.Interact();

        return PickUp();

    }

    private bool PickUp()
    {
        
        bool wasPickedUp = Inventory.instance.Add(item);
        if(wasPickedUp)
            Destroy(gameObject);
        return wasPickedUp;
    }
}
