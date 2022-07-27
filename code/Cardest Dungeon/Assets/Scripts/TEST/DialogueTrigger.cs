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

    public void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
        dm.read[textnumber] = textnumber;
        if(audioSource!=null)
        audioSource.Play();
    }

    public void TriggerNPC()
    {
        dm.StartNPC(dialogue);
        dm.read[textnumber] = textnumber;
        if (textnumber == 10)
        {
            StartCoroutine(ChangeSprite());
        }
    }

    IEnumerator ChangeSprite()
    {
        yield return new WaitForSeconds(2.0f);
        if (fakeWife != null)

        {
            GetComponent<SpriteRenderer>().sprite = fakeWife;
        }

    }
}
