using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class animates the player icon on the map
/// </summary>
public class PlayerIcon : MonoBehaviour
{
    public static PlayerIcon Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerIcon>();
            }
            return instance;
        }
    }

    private static PlayerIcon instance;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PauseAnimation();
    }

    public void PauseAnimation()
    {
        if (animator != null)
        {
            animator.speed = 0;
            animator.Play(0, -1, 0);
        }
    }

    public void UnpauseAnimation()
    {
        if (animator != null)
        {
            animator.speed = 1;
        }
    }
}
