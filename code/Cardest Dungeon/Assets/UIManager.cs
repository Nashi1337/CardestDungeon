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
    private Text restartText;
    private bool isGameOver = false;

    void Start()
    {
        gameoverpanel.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && !isGameOver)
        {
            isGameOver = true;
            StartCoroutine(GameOverSequence());
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }
        }
    }

        
        private IEnumerator GameOverSequence()
             {
        
                gameoverpanel.SetActive(true);
        
            yield return new WaitForSeconds(5.0f);

            restartText.gameObject.SetActive(true);
            }
}
