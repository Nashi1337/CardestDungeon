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

    [SerializeField]
    private GameObject playerIcon;

    private MapPiece[] allMapPieces = null;
    private static MapManager current = null;
    private GameObject player;

    private void OnEnable()
    {
        FindAllMapPieces(); //OnCollisionEnter seems to trigger before Start. So Map Pieces not to be found before Start. "Danke, Henrik -_-"
    }

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        //UpdatePlayerPiece();
    }

    private void Update()
    {
        //UpdatePlayerPiece();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapPiece">The map piece at which the player icon will be set.</param>
    public void UpdatePlayerPiece(MapPiece mapPiece)
    {
        //MapPiece closestPiece = FindClosestMapPieceByPosition(player.transform.position);
        //Debug.Log(closestPiece);

        playerIcon.transform.SetParent(mapPiece.transform, false);
    }

    private void FindAllMapPieces()
    {
        allMapPieces = FindObjectsOfType<MapPiece>();
    }

    /// <summary>
    /// Searches for the nearest map piece and returns it.
    /// </summary>
    /// <param name="pos"></param>
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
    /// <returns>Returns true if swap succeded. Else false</returns>
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
    /// Search for MapPiece by its dungeon parts. "Danke, Henrik -_-"
    /// </summary>
    /// <param name="dungeonPart"></param>
    /// <returns></returns>
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
