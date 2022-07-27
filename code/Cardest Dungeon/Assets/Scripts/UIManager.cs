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
    //[SerializeField]
    //private Button continueButton;
    public bool isGameOver = false;

    public static UIManager instance;

    void Start()
    {
        instance = this;
        gameoverpanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        //continueButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
/*        if(Input.GetKeyDown(KeyCode.G) && !isGameOver)
        {
            isGameOver = true;
        }*/

        if (isGameOver)
        {
            StartCoroutine(GameOverSequence());
/*            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }*/
        }
    }

        
    private IEnumerator GameOverSequence()
    {
        
    gameoverpanel.SetActive(true);
        
    yield return new WaitForSeconds(3.0f);

    restartButton.gameObject.SetActive(true);
    quitButton.gameObject.SetActive(true);
    }


    
    
}
