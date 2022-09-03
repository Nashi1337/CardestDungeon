using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : Interactable
{
    public Item item;
    public bool bossItem;
    public bool tutorialItem;
    public GameObject audioPlayerPrefab;
    public AudioSource itemPickup;
    public AudioSource bossItemPickup;

    /// <summary>
    /// Items have this function that will be called when interacted with and returns a call to the PickUp function
    /// </summary>
    /// <returns></returns>
    public override bool Interact()
    {

        base.Interact();

        return PickUp();

    }
    /// <summary>
    /// Differentiates between special items that trigger a new scene and normal items
    /// </summary>
    /// <returns>A bool that signifies wether the item was sucessfully picked up or not</returns>
    private bool PickUp()
    {
        //the picking up happens in the inventory class. wasPickedUp is the return value, in this case of both functions.
        bool wasPickedUp = Inventory.instance.Add(item);

        //the first boss drops a special item, that triggers a scene transition upon pickup
        if (bossItem&&wasPickedUp)
        {
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            audioPlayer.GetComponent<AudioSource>().clip = bossItemPickup.clip;
            StartCoroutine(LoadNextScene());
            PlayerController.canMove = true;
            return wasPickedUp;
        }
        //the last enemy of the tutorial drops a special item as well, that loads the first dungeon scene
        else if (tutorialItem && wasPickedUp)
        {
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            audioPlayer.GetComponent<AudioSource>().clip = bossItemPickup.clip;
            SceneManager.LoadScene(0);
            return wasPickedUp;
        }
        else
        {
            //if the item is not special, this block will be called, and nothing special happens
            if (wasPickedUp)
            {
                GameObject audioPlayer = Instantiate(audioPlayerPrefab);
                audioPlayer.GetComponent<AudioSource>().clip = itemPickup.clip;
                Destroy(gameObject);

            }
        return wasPickedUp;
        }
    }
    /// <summary>
    /// Loads the next scene and saves the inventory before doing so.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);
        Inventory.instance.SaveInventoryToPlayerStats();
        SceneManager.LoadScene(0);
    }
}
