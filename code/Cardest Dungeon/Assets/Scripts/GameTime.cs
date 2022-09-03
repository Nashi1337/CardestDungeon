using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static bool IsGamePaused { get { return isGamePaused; } set { isGamePaused = value; if (value) { Time.timeScale = 0; } else { Time.timeScale = 1; } } }


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
    /// -Is the pause menu opened?
    /// </summary>
    public static void UpdateIsGamePaused()
    {
        Debug.Log(Inventory.instance);
        Debug.Log(MapManager.Current);
        Debug.Log(Pause.instance);
        isGamePaused = Inventory.instance.gameObject.activeInHierarchy 
                    || MapManager.Current.gameObject.activeInHierarchy
                    || Pause.instance.gameObject.activeInHierarchy;

        Time.timeScale = isGamePaused ? 0 : 1;

        if (isGamePaused)
        {
            if(waterScript!=null)
                waterScript.StopWaterSound();
        }
    }
}
