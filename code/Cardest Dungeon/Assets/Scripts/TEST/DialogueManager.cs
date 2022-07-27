using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //For longer dialogue boxes we need a FIFO queue to store the text.
    public Queue<string> sentences;

    public Text nameText;
    public TMP_Text dialogue;
    public Text dialogue2;
    public Animator animator;
    public AudioSource audio;
    public float textSpeed;

    PlayerController pc;

    public int[] read;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue1)
    {
        GameTime.UpdateIsGamePaused();
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue1.name;

        sentences.Clear();
        


        foreach(string sentence in dialogue1.sentences)
        {
            if(sentence != dialogue2.text)
                sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void StartNPC(Dialogue dialogue)
    {
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            if (sentence != dialogue2.text)
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
        dialogue2.text = "";
        audio.Play();
        foreach(char letter in sentence.ToCharArray())
        {
            dialogue2.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        audio.Stop();
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
        animator.SetBool("IsOpen", false);
        PlayerController.canMove = true;

        if (read[11] == 11)
        { 
            //Initiate credits
            GameTime.IsGamePaused = true;
            FadeOutToCredits fff = gameObject.AddComponent<FadeOutToCredits>();
            fff.timeToFadeOut = 5;
        }
    }

    public void CustomDialogue(string sentence)
    {
        nameText.text = "Tutorial";
        //PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        dialogue2.text = sentence;
    }

    public void Tutorial1()
    {
        nameText.text = "Tutorial #1";
        PlayerController.canMove = false;
        animator.SetBool("IsOpen", true);
        dialogue2.text = "Press \"W\",\"A\",\"S\",\"D\" to walk and \"E\" to interact with objects.";
    }

    public void Victory()
    {
        nameText.text = "Victory";
        animator.SetBool("IsOpen", true);
        dialogue2.text = "You did it! Congratulations! Press \"Q\" 15 times to exit the game!";
    }

    public void NextDungeon()
    {
        nameText.text = "Proceed";
        //animator noch nötig?
        dialogue2.text = "You defeated Herbert the dragon. Pick up your reward to enter the next dungeon!";
    }

    public void RedKey()
    {
        nameText.text = "Red key";
        animator.SetBool("IsOpen", true);
        dialogue2.text = "You have found the red key!";
    }
    public void BlueKey()
    {
        nameText.text = "Blue key";
        animator.SetBool("IsOpen", true);
        dialogue2.text = "You have found the blue key!";
    }

    public void RedDoor()
    {
        nameText.text = "Red door";
        animator.SetBool("IsOpen", true);
        dialogue2.text = "You have opened the red door!";
    }

    public void BlueDoor()
    {
        nameText.text = "Blue door";
        animator.SetBool("IsOpen", true);
        dialogue2.text = "You have opened the blue door!";
    }
}
