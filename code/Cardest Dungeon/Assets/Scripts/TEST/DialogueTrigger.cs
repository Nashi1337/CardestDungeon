using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dm;
    public int textnumber=0;
    public KeyCode key;
    AudioSource audioSource;

    [SerializeField]
    private Sprite fakeWife;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Will be called when interacting with NPCs or certain objects.
    /// </summary>
    public void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
        //Some objects have a textnumber.
        //If they do, they write in the read array of dialoguemanager at the same position that the value represents
        //Was mainly used to check if the player talked to NPCs
        dm.read[textnumber] = textnumber;
        if(audioSource!=null)
        audioSource.Play();
    }
    /// <summary>
    /// same as TriggerDialogue, only special for NPCs.
    /// </summary>
    public void TriggerNPC()
    {
        dm.StartNPC(dialogue);
        dm.read[textnumber] = textnumber;
        //a certain NPC has his Sprite changed upon talking to.
        if (textnumber == 10)
        {
            StartCoroutine(ChangeSprite());
        }
    }
    /// <summary>
    /// Changes the sprite of a certain NPC
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeSprite()
    {
        yield return new WaitForSeconds(2.0f);
        if (fakeWife != null)

        {
            GetComponent<SpriteRenderer>().sprite = fakeWife;
        }

    }
}
