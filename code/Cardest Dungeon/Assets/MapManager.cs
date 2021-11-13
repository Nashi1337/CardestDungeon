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

    private void FindAllMapPieces()
    {
        allMapPieces = FindObjectsOfType<MapPiece>();
    }

    ///// <summary>
    ///// Checks if the map piece at the given position is unlocked.
    ///// </summary>
    ///// <param name="position"></param>
    ///// <returns>True, if a map piece at the given position was found. False, if no map piece at given position was found.</returns>
    //public bool IsMapPieceUnlocked(Vector3 position)
    //{
    //    foreach(MapPiece mapPiece in allMapPieces)
    //    {
    //        if(mapPiece.transform.position == position && mapPiece.IsUnlocked)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

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

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="position"></param>
    ///// <returns>Returns the MapPiece at given position or null if nothing was found.</returns>
    //public MapPiece FindMapPiece(Vector3 position)
    //{
    //    foreach (MapPiece mapPiece in allMapPieces)
    //    {
    //        if (mapPiece.transform.position == position)
    //        {
    //            return mapPiece;
    //        }
    //    }
    //    return null;
    //}
}
