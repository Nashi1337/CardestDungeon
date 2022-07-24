using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutToCredits : MonoBehaviour
{
    public float timeToFadeOut;

    private SpriteRenderer tintPlane;

    // Start is called before the first frame update
    void Start()
    {
        //Disable player input
        InputManager.IgnoreInput();

        //One of the most ugly lines here probably
        tintPlane = PlayerController.Current.GetComponentInChildren<CameraTintScript>().transform.GetChild(0).GetComponent<SpriteRenderer>();
        Color col = Color.black;
        col.a = 0;
        tintPlane.color = col;

        StartCoroutine(NextGradient());
    }

    private IEnumerator NextGradient()
    {
        Color col = tintPlane.color;
        if (col.a >= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        col.a += 0.2f / timeToFadeOut;
        tintPlane.color = col;

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(NextGradient());
    }
}
