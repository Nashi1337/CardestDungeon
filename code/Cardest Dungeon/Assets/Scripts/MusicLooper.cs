using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    public static MusicLooper Instance;
    public bool IsActive { get { return isActive; } }
    public AudioSource GameMusic { get { return source; } }

    private bool isActive = false;
    private float loopStart;
    private float loopEnd;

    private AudioClip audioClip;
    private AudioSource source;

    private bool isFadingOut = false;
    private float startingMusicVolume;
    private float fadeOut = 1;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (source.timeSamples >= loopEnd * audioClip.frequency)
            {
                source.timeSamples = Mathf.RoundToInt(loopStart * audioClip.frequency);
            }
        }

        if(isFadingOut)
        {
            fadeOut -= Time.unscaledDeltaTime / 2.5f;
            if (source.volume > 0)
                source.volume = startingMusicVolume * fadeOut;
            else
                isFadingOut = false;
        }
    }

    public void ActivateLooper(float loopStart, float loopEnd)
    {
        isActive = true;

        this.loopStart = loopStart;
        this.loopEnd = loopEnd;
        audioClip = source.clip;
    }

    public void FadeOutMusic()
    {
        isFadingOut = true;
        startingMusicVolume = source.volume;
    }
}
