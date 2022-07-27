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
    private GameObject PauseButton;
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        Debug.Log("Hallo ich bin Start von Pause");
        Initialize();
    }

    public void Initialize()
    {
        SetVolume(0.4f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseWindow()
    {
        GameTime.IsGamePaused = false;
        PauseMenu.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
