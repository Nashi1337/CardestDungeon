using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    #region Singleton

    public static Pause instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

    }

    #endregion

    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    ///Initialize will be called right at the start of the game.
    ///It sets the volume to 0.4 out of 1 because everything above seemed a bit loud.
    /// </summary>

    public void Initialize()
    {
        SetVolume(0.4f);
    }

    /// <summary>
    ///Pressing the Quit Button on the Pause Menu quits the game and closes the window. 
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// This closes the pause menu and continues the game.
    /// </summary>
    public void CloseWindow()
    {
        GameTime.IsGamePaused = false;
        PauseMenu.gameObject.SetActive(false);
    }
    /// <summary>
    /// The Main Menu is Scene 0. Pressing the Button for the Main Menu loads this scene. 
    /// All progress will naturally be lost.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// AudioListener controls the audio for all sounds and music in all scenes.
    /// Changing the value via the slider in the Pause Menu sets the volume to a value between 0 and 1
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
