using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages everything that has to do with changinge the dungeon layout and controlling the visual map.
/// </summary>
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

    [SerializeField]
    private GameObject playerIcon;

    private MapPiece[] allMapPieces = null;
    private static MapManager current = null;
    private GameObject player;

    private void OnEnable()
    {
        //OnCollisionEnter seems to trigger before Start. So Map Pieces will not to be found before Start. "Danke, Henrik -_-"
        FindAllMapPieces(); 
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Changes the player location on the visual map to mapPiece by setting the parent of the player icon.
    /// </summary>
    /// <param name="mapPiece">The map piece at which the player icon will be appended.</param>
    public void UpdatePlayerPiece(MapPiece mapPiece)
    {

        playerIcon.transform.SetParent(mapPiece.transform, false);
    }

    /// <summary>
    /// Searches the scene for every instance of the class MapPiece and adds them to the allMapPieces list.
    /// </summary>
    private void FindAllMapPieces()
    {
        allMapPieces = FindObjectsOfType<MapPiece>();
    }

    /// <summary>
    /// Searches for the closest map piece to pos and returns it.
    /// </summary>
    /// <param name="pos">The position from which the closest map piece will be calculated.</param>
    /// <returns>Returns the closest mapPiece to pos.</returns>
    private MapPiece FindClosestMapPieceByPosition(Vector3 pos)
    {

        float shortestDistance = float.MaxValue;
        int index = 0;

        for (int i = 0; i < allMapPieces.Length; i++)
        {
            float currentDistance = (pos - allMapPieces[i].dungeonPart.transform.position).magnitude;

            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                index = i;
            }

            Debug.Log(currentDistance + " "+ allMapPieces[i]);
        }
        return allMapPieces[index];
    }

    /// <summary>
    /// Searches for the closest map piece to mapPiece that is within distanceCap and returns it. This is intended for use within
    /// the MapPiece class only.
    /// </summary>
    /// <param name="mapPiece">The map piece from which the closest other map piece will be calculated</param>
    /// <param name="distanceCap">Maximum distance of any map piece to the given one in pixels</param>
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
    /// Swaps two map pieces with each other if they are both unlocked.
    /// </summary>
    /// <param name="piece1">First piece to swap.</param>
    /// <param name="piece2">Second piece to swap.</param>
    /// <returns>True if swap succeded. Else false</returns>
    public bool SwapMapPieces(MapPiece piece1, MapPiece piece2)
    {
        MapPiece playersLocation = playerIcon.transform.parent.GetComponent<MapPiece>();
        if (playersLocation.Equals(piece1) || playersLocation.Equals(piece2))
        {
            return false;
        }

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

    /// <summary>
    /// Search for MapPiece by its dungeon parts. Needed for updating player icon on visual map correctly. "Danke, Henrik -_-"
    /// </summary>
    /// <param name="dungeonPart">The dungeon part whos corresponding map piece will be looked for</param>
    /// <returns>The corresponding mapPiece do dungeonPart or null if nothing was found.</returns>
    public MapPiece SearchMapPiece(GameObject dungeonPart)
    {
        foreach(MapPiece mapPiece in allMapPieces)
        {
            if (mapPiece.dungeonPart.Equals(dungeonPart))
            {
                return mapPiece;
            }
        }

        return null;
    }
}
