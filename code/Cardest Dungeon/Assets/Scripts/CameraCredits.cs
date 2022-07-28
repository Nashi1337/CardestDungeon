using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the credits
/// </summary>
public class CameraCredits : MonoBehaviour
{
    [SerializeField]
    GameObject scroller;
    [SerializeField]
    Text highscore;
    [SerializeField]
    private float scrollingSpeed;
    [SerializeField]
    private float stopAfterUnits;

    private float stopAfterSeconds;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        stopAfterSeconds = stopAfterUnits / scrollingSpeed;

        highscore.text += " " + PlayerPrefs.GetString("HighScore");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad < stopAfterSeconds)
        {
            scroller.transform.position += new Vector3(0, scrollingSpeed * Time.fixedDeltaTime, 0);
        }
    }
}
