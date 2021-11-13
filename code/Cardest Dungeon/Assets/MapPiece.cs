using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsUnlocked
    {
        get { return isUnlocked; }
        set { isUnlocked = value; }
    }

    private bool isUnlocked = true;
    private Vector3 positionBeforeDrag = default;
    private MapManager mapManager = null;

    //Maybe change this to something else than a static variable in MapPiece
    private static int maxDistanceForSnapping = 50;
    private void Start()
    {
        mapManager = MapManager.Current;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        positionBeforeDrag = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MapPiece switchPiece = mapManager.FindClosestMapPiece(this, maxDistanceForSnapping);

        if (switchPiece.IsUnlocked)
        {
            transform.position = switchPiece.transform.position;
            switchPiece.transform.position = positionBeforeDrag;
        }
        else
        {
            transform.position = positionBeforeDrag;
        }
    }

}
