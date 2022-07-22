using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static bool IsGamePaused { get { return isGamePaused; } set { isGamePaused = value; } }

    private static bool isGamePaused = false;

    private static WaterScript waterScript;

    // Start is called before the first frame update
    void Start()
    {
        waterScript = FindObjectOfType<WaterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks the following conditions. If any of these are true the game will be paused and GameTime.IsGamePaused will be set to true. Else the game will continue and GameTime.IsGamePaused will be set to false:
    /// -Is the inventory Menu opened?
    /// -Is the map menu opened?
    /// </summary>
    public static void UpdateIsGamePaused()
    {
        isGamePaused = Inventory.instance.gameObject.activeInHierarchy 
                    || MapManager.Current.gameObject.activeInHierarchy;

        Time.timeScale = isGamePaused ? 0 : 1;

        if (isGamePaused)
        {
            if(waterScript!=null)
                waterScript.StopWaterSound();
        }
    }
}
