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

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
        dm.read[textnumber] = textnumber;
        audioSource.Play();
    }
}
