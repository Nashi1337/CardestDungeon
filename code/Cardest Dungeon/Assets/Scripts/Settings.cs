using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    private GameObject lastSelection;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SetVolume(0.4f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //lastSelection = irgendwas mit ray cast?
        }
    }

    /// <summary>
    /// Sets the volume of audio
    /// </summary>
    /// <param name="volume">The volume to whichthe audio will be set.</param>
    public void SetVolume(float volume)
    { 
        AudioListener.volume = volume;
    }

    /// <summary>
    /// Sets the quality level of the graphics.
    /// </summary>
    /// <param name="qualityIndex">The level of quality the grpahics will be set.</param>
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Sets the game to fullscreen or to window mode.
    /// </summary>
    /// <param name="isFullscreen">true = game will be set to fullscreen mode. False = Game will be set to window mode.</param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// Sets the resolution of the game window.
    /// </summary>
    /// <param name="resolutionIndex">The index of the resolution to which the game resolution will be set.</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
