using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCredits : MonoBehaviour
{
    [SerializeField]
    GameObject scroller;
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
