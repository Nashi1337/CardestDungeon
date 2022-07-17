using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : Interactable
{
    public Item item;
    public bool bossItem;
    public override bool Interact()
    {

        base.Interact();

        return PickUp();

    }

    private bool PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);

        if (bossItem&&wasPickedUp)
        {
            LoadNextScene();
            return wasPickedUp;
        }
        else
        {

        if(wasPickedUp)
            Destroy(gameObject);
        return wasPickedUp;
        }
    }

    private void LoadNextScene()
    {
        //yield return new WaitForSeconds(3);
        Inventory.instance.SaveInventoryToPlayerStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
