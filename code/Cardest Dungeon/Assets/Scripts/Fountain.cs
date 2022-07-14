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

    private void ChangeSprite()
    {
        Element newElement = MapManager.Current.FindElementOfMapPiece(homeArea);
        Debug.Log(newElement);

        animator.SetInteger("Element", (int)newElement);
    }
}
