using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public AudioSource audio;
    public float textSpeed;

    public int[] read;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    /// <summary>
    /// Every Dialogue Trigger calls this method.
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        //Pause game so that while the player is unable to move he can't get hurt.
        GameTime.UpdateIsGamePaused();
        PlayerController.canMove = false;
        //This moves the text box into the visible area
        animator.SetBool("IsOpen", true);
        //dialogue contains a name to put on top of the text box, so that the player knows who's talking
        nameText.text = dialogue.name;

        //the sentences Queue must be cleared before each dialogue to remove previous dialogues.
        sentences.Clear();
        

        //after clearing, the queue will be filled with all strings delivered through "dialogue".
        foreach(string sentence in dialogue.sentences)
        {
            if(sentence != dialogueText.text)
                sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// This special function will be called if the owner of a dialogue has the tag "NPC".
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartNPC(Dialogue dialogue)
    {
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            if (sentence != dialogueText.text)
                sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Will be called to initialize the dialogue and from the "OK" button in the textbox
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //To skip dialogue, players can press "OK". This stops the typing effect.
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Types the dialogue in a customizable speed. The String will be divided into a char array, and every char will be placed with a delay of default 0.05 seconds.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        audio.Play();
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        audio.Stop();
    }
    /// <summary>
    /// When the dialogue is exhausted, the text box disappears and the player can move again
    /// </summary>
    void EndDialogue()
    {
        //Debug.Log("End of conversation");
        animator.SetBool("IsOpen", false);
        PlayerController.canMove = true;

        //This triggers the transition to the Credit scene. It will be called, when a certain NPC is talked to. It sets the value of the "read" array at position 11 to 11.
        if (read[11] == 11)
        { 
            //Initiate credits
            GameTime.IsGamePaused = true;
            FadeOutToCredits fff = gameObject.AddComponent<FadeOutToCredits>();
            fff.timeToFadeOut = 5;
        }
    }

    /// <summary>
    /// In the following are custom dialogues that will be triggered on special events 
    /// if the object that would normally trigger them is already destroyed.
    /// </summary>
    public void Tutorial1()
    {
        nameText.text = "Tutorial #1";
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        dialogueText.text = "Press \"W\",\"A\",\"S\",\"D\" to walk and \"E\" to interact with objects.";
    }

    public void NextDungeon()
    {
        nameText.text = "Proceed";
        animator.SetBool("IsOpen", true);
        dialogueText.text = "You defeated Herbert the dragon. Pick up your reward to enter the next dungeon!";
    }

    public void RedKey()
    {
        nameText.text = "Red key";
        animator.SetBool("IsOpen", true);
        dialogueText.text = "You have found the red key!";
    }
    public void BlueKey()
    {
        nameText.text = "Blue key";
        animator.SetBool("IsOpen", true);
        dialogueText.text = "You have found the blue key!";
    }

    public void RedDoor()
    {
        nameText.text = "Red door";
        animator.SetBool("IsOpen", true);
        dialogueText.text = "You have opened the red door!";
    }

    public void BlueDoor()
    {
        nameText.text = "Blue door";
        animator.SetBool("IsOpen", true);
        dialogueText.text = "You have opened the blue door!";
    }
}
