using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameoverpanel;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button quitButton;
    public bool isGameOver = false;

    public static UIManager instance;

    void Start()
    {
        instance = this;
        gameoverpanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    /// <summary>
    /// When the player's health reaches 0, this Method will be called.
    /// It actives the game over panel, and after 3 seconds displays options to restart or to quit the game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverSequence()
    {
        
    gameoverpanel.SetActive(true);
        
    yield return new WaitForSeconds(3.0f);

    restartButton.gameObject.SetActive(true);
    quitButton.gameObject.SetActive(true);
    }


    
    
}
