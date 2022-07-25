using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the scene with the main gameplay. 
    /// </summary>
    public void PlayGame()
    {
        //The scene should not be loaded by scene index.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Jemand hat Quit geklickt");
        Application.Quit();
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(4);
    }
}
