using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// loads the first dungeon
    /// </summary>
    public void PlayGame()
    {
        GameTime.IsGamePaused = false;
        SceneManager.LoadScene(2);
        PlayerPrefs.DeleteKey("Inventory");
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Jemand hat Quit geklickt");
        Application.Quit();
    }
    /// <summary>
    /// loads the tutorial scene
    /// </summary>
    public void StartTutorial()
    {
        GameTime.IsGamePaused = false;
        SceneManager.LoadScene(1);
    }
}
