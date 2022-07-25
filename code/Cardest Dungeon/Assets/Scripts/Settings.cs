using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// This class handles ingame settings like audio volume and screen resolution.
/// </summary>
public class Settings : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
   

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

        SetVolume(0.5f);
    }

    /// <summary>
    /// Sets the volume of audio
    /// </summary>
    /// <param name="volume">The volume to whichthe audio will be set.</param>
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        //audioMixer.SetFloat("Volume", volume);
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
