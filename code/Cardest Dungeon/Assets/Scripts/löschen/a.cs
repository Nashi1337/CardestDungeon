using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveData : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Saved") == 1 && PlayerPrefs.GetInt("TimeToLoad") == 1)
        {
            float pX = player.transform.position.x;
            float pY = player.transform.position.y;

            pX = PlayerPrefs.GetFloat("p_x");
            pY = PlayerPrefs.GetFloat("p_y");
            player.transform.position = new Vector2(pX, pY);

            PlayerPrefs.SetInt("TimeToLoad", 0);
            PlayerPrefs.Save();
        }    
    }

    public void PlayerPosSave(Vector2 playerPosition)
    {
        /*        PlayerPrefs.SetFloat("p_x", player.transform.position.x);
                PlayerPrefs.SetFloat("p_y", player.transform.position.y);
                PlayerPrefs.SetInt("Saved", 1);

                PlayerPrefs.Save();*/

        playerStorage.initialValue = playerPosition;
    }

    public void PlayerPosLoad()
    {
        PlayerPrefs.SetInt("TimeToLoad", 1);
        PlayerPrefs.Save();
    }
}
