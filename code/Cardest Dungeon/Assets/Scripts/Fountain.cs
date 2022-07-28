using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour, IDungeonSwapMessage
{
    private Animator animator;
    private MapPiece homeArea;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        homeArea = MapManager.Current.FindClosestMapPieceByPosition(transform.position);
        ChangeSprite();
    }

    public void OnDungeonSwap()
    {
        ChangeSprite();
    }

    /// <summary>
    /// Disappears in NONE element, switches to blue in ICE and to red in FIRE area
    /// </summary>
    private void ChangeSprite()
    {
        Element newElement = MapManager.Current.FindElementOfMapPiece(homeArea);
        animator.SetInteger("Element", (int)newElement);

        float animSpeed = 1;
        switch(newElement)
        {
            case Element.FIRE:
                animSpeed = 1;
                break;
            case Element.ICE:
                animSpeed = 0.6f;
                break;
        }

        animator.speed = animSpeed;
    }
}
