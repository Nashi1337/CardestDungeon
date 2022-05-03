using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dm;
    public int textnumber=0;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
        dm.read[textnumber] = textnumber;
    }
}
