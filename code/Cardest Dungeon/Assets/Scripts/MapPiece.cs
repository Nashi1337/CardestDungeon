using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// represents a piece on the map and of the map of a dungeon. It has a reference on its corresponding actual dungeonPart.
/// </summary>
public class MapPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsUnlocked
    {
        get { return isUnlocked; }
        set
        {
            if (value)
            {
                GetComponent<Image>().color = Color.white;
            }
            isUnlocked = value;
        }
    }
    public Vector3 DungeonPartPosition { get { return dungeonPiecePosition; } }
    public Vector3 PositionBeforeDrag { get { return positionBeforeDrag; } }

    public GameObject dungeonPart;

    private bool isUnlocked = false;
    private Vector3 positionBeforeDrag = default;
    private Vector3 dungeonPiecePosition = default;
    private MapManager mapManager = default;

    //Maybe change this to something else than a static variable in MapPiece
    private static int maxDistanceForSnapping = 50;
    private void Start()
    {
        IsUnlocked = Debug.isDebugBuild;
        Debug.LogWarning("All MapPieces are unlocked in Debug builds. Remove this line before release!!");

        positionBeforeDrag = transform.position;
        dungeonPiecePosition = dungeonPart.transform.position;
        mapManager = MapManager.Current;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsUnlocked)
        {
            positionBeforeDrag = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsUnlocked)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MapPiece other = mapManager.FindClosestMapPiece(this, maxDistanceForSnapping);

        //If pieces could not be swapped.
        if(other == null || !mapManager.SwapMapPieces(this, other))
        {
            transform.position = positionBeforeDrag;
        }
    }

    /// <summary>
    /// Changes the position of this MapPiece and its underlying Dungeon part.
    /// </summary>
    /// <param name="newMapPosition">The position for the MapPiece will be set to this.</param>
    /// <param name="newDungeonPosition">The position for the DungeonPart will be set to this.</param>
    public void ChangePosition(Vector3 newMapPosition, Vector3 newDungeonPosition)
    {
        positionBeforeDrag = transform.position = newMapPosition;
        dungeonPart.transform.position = newDungeonPosition;
        dungeonPiecePosition = newDungeonPosition;
    }

}
