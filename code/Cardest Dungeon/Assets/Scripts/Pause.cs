using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
           // GameTime.UpdateIsGamePaused();
        }
    }

}
