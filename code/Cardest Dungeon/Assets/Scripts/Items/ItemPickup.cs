using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : Interactable
{
    public Item item;
    public bool bossItem;
    public GameObject audioPlayerPrefab;
    public AudioSource itemPickup;
    public AudioSource bossItemPickup;

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
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            audioPlayer.GetComponent<AudioSource>().clip = bossItemPickup.clip;
            StartCoroutine(LoadNextScene());
            return wasPickedUp;
        }
        else
        {

            if (wasPickedUp)
            {
                GameObject audioPlayer = Instantiate(audioPlayerPrefab);
                audioPlayer.GetComponent<AudioSource>().clip = itemPickup.clip;
                Destroy(gameObject);

            }
        return wasPickedUp;
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);
        Inventory.instance.SaveInventoryToPlayerStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
