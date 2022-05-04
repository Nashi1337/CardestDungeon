using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //For longer dialogue boxes we need a FIFO queue to store the text.
    public Queue<string> sentences;

    public Text nameText;
    public Text dialogue;

    public Animator animator;

    //PlayerController pc;

    public int[] read;

    // Start is called before the first frame update
    void Start()
    {
        //pc = FindObjectOfType<PlayerController>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogue.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogue.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
        animator.SetBool("IsOpen", false);
        PlayerController.canMove = true;
    }

    public void CustomDialogue(string sentence)
    {
        nameText.text = "Tutorial";
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        dialogue.text = sentence;
    }

    public void Tutorial1()
    {
        nameText.text = "Tutorial";
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        dialogue.text = "Press \"W\",\"A\",\"S\",\"D\" to walk and \"E\" to interact with objects.";
    }
}
