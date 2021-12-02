using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<MapManager>();
                if(current == null)
                {
                    Debug.LogError("Could not find a Mapmanager. Did you forget to add one to the scene?");
                }
            }
            return current;
        }
    }

    private Vector2Int playerPiece = default;
    private MapPiece[] allMapPieces = null;
    private static MapManager current = null;
    // Start is called before the first frame update
    void Start()
    {
        FindAllMapPieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerPiece()
    {

    }

    private void FindAllMapPieces()
    {
        allMapPieces = FindObjectsOfType<MapPiece>();
    }

    /// <summary>
    /// Searches for the nearest map piece and returns it.
    /// </summary>
    /// <param name="mapPiece"></param>
    /// <param name="distanceCap">maximum distance of any map piece to the given one in pixels</param>
    /// <returns>If there is no map piece closer than distanceCap it will return mapPiece. Otherwise the closest map piece.</returns>
    public MapPiece FindClosestMapPiece(MapPiece mapPiece, float distanceCap)
    {

        float shortestDistance = float.MaxValue;
        int index = 0;

        for(int i = 0; i < allMapPieces.Length; i++)
        {
            float currentDistance = (mapPiece.transform.position - allMapPieces[i].transform.position).magnitude;
            
            if(currentDistance < shortestDistance && mapPiece != allMapPieces[i])
            {
                shortestDistance = currentDistance;
                index = i;
            }
        }

        if (shortestDistance < distanceCap)
        {
            return allMapPieces[index];
        }
        else
        {
            return mapPiece;
        }
    }
    /// <summary>
    /// Swaps two map pieces with each other if they are both unlocked
    /// </summary>
    /// <param name="piece1"></param>
    /// <param name="piece2"></param>
    /// <returns></returns>
    public bool SwapMapPieces(MapPiece piece1, MapPiece piece2)
    {
        if (piece1.IsUnlocked && piece2.IsUnlocked)
        {
            Vector3 piece1Pos = piece1.PositionBeforeDrag;
            Vector3 piece1DungeonPos = piece1.DungeonPartPosition;
            
            piece1.ChangePosition(piece2.PositionBeforeDrag, piece2.DungeonPartPosition);
            piece2.ChangePosition(piece1Pos, piece1DungeonPos);

            return true;
        }
        return false;
    }
}
