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
                    //Debug.LogError("Could not find a Mapmanager. Did you forget to add one to the scene?");
                }
            }
            return current;
        }
    }

    private MapPiece playerPosition;
    private MapPiece[,] allMapPieces = null;
    private static MapManager current = null;
    private List<WaterScript> allSources;
    private DungeonAttributes dungeonAttributes;

    private void Start()
    {
        allSources = new List<WaterScript>();
        foreach (WaterScript water in FindObjectsOfType<WaterScript>())
        {
            allSources.Add(water);
        }

    }

    private void OnEnable()
    {
        current = FindObjectOfType<MapManager>();
        dungeonAttributes = FindObjectOfType<DungeonAttributes>();
        //OnCollisionEnter seems to trigger before Start. So Map Pieces will not to be found before Start. "Danke, Henrik -_-"
        FindAllMapPieces(); 
    }

    /// <summary>
    /// Changes the player location on the visual map to mapPiece by setting the parent of the player icon.
    /// </summary>
    /// <param name="mapPiece">The map piece at which the player icon will be appended.</param>
    public void UpdatePlayerPosition(MapPiece mapPiece)
    {
        playerPosition = mapPiece;
    }

    /// <summary>
    /// Searches the scene for every instance of the class MapPiece and adds them to the allMapPieces list.
    /// </summary>
    private void FindAllMapPieces()
    {
        allMapPieces = new MapPiece[dungeonAttributes.DungeonWidth, dungeonAttributes.DungeonHeight];

        MapPiece[] allMapPiecesUnsorted = FindObjectsOfType<MapPiece>();
        foreach(MapPiece mapPiece in allMapPiecesUnsorted)
        {
            (int, int) pos = ExtractdungeonPartPosition(mapPiece.dungeonPart);
            allMapPieces[pos.Item1, pos.Item2] = mapPiece;
        }
    }

    /// <summary>
    /// Searches for the closest map piece to pos and returns it.
    /// </summary>
    /// <param name="pos">The position from which the closest map piece will be calculated.</param>
    /// <returns>Returns the closest mapPiece to pos.</returns>
    public MapPiece FindClosestMapPieceByPosition(Vector3 pos)
    {

        float shortestDistance = float.MaxValue;
        int indexi = 0;
        int indexj = 0;

        for (int i = 0; i < allMapPieces.GetLength(0); i++)
        {
            for (int j = 0; j < allMapPieces.GetLength(1); j++)
            {
                float currentDistance = (pos - allMapPieces[i, j].dungeonPart.transform.position).magnitude;

                if (currentDistance < shortestDistance)
                {
                    shortestDistance = currentDistance;
                    indexi = i;
                    indexj = j;
                }
            }
        }
        return allMapPieces[indexi, indexj];
    }

    /// <summary>
    /// Extracts the position written in the name. E.g.: 1_0 ==> (1,0)
    /// </summary>
    /// <param name="dungeonPart"></param>
    /// <returns></returns>
    private (int, int) ExtractdungeonPartPosition(GameObject dungeonPart)
    {
        char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        int indexOfX = dungeonPart.name.IndexOfAny(numbers);
        int indexOfY = indexOfX + 2;

        (int, int) pos;
        pos.Item1 = int.Parse(dungeonPart.name[indexOfX].ToString());
        pos.Item2 = int.Parse(dungeonPart.name[indexOfY].ToString());

        return pos;
    }

    private (int, int) FindMapPiecePositionInArray(MapPiece mapPiece)
    {
        for (int i = 0; i < allMapPieces.GetLength(0); i++)
        {
            for (int j = 0; j < allMapPieces.GetLength(1); j++)
            {
                if (mapPiece == allMapPieces[i, j])
                {
                    return (i, j);
                }
            }
        }

        throw new System.Exception("mapPiece could not be found");
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
        int indexi = 0;
        int indexj = 0;

        for(int i = 0; i < allMapPieces.GetLength(0); i++)
        {
            for (int j = 0; j < allMapPieces.GetLength(1); j++)
            {
                float currentDistance = (mapPiece.transform.position - allMapPieces[i, j].transform.position).magnitude;

                if (currentDistance < shortestDistance && mapPiece != allMapPieces[i, j])
                {
                    shortestDistance = currentDistance;
                    indexi = i;
                    indexj = j;
                }
            }
        }

        if (shortestDistance < distanceCap)
        {
            return allMapPieces[indexi, indexj];
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
        if(piece1 == piece2)
        {
            return false;
        }

        if (playerPosition.Equals(piece1) || playerPosition.Equals(piece2))
        {
            return false;
        }

        if (piece1.IsUnlocked && piece2.IsUnlocked)
        {
            Vector3 piece1Pos = piece1.PositionBeforeDrag;
            Vector3 piece1DungeonPos = piece1.DungeonPartPosition;
            
            piece1.ChangePosition(piece2.PositionBeforeDrag, piece2.DungeonPartPosition);
            piece2.ChangePosition(piece1Pos, piece1DungeonPos);

            (int, int) pos1 = FindMapPiecePositionInArray(piece1);
            (int, int) pos2 = FindMapPiecePositionInArray(piece2);
            MapPiece temp = allMapPieces[pos1.Item1, pos1.Item2];
            allMapPieces[pos1.Item1, pos1.Item2] = allMapPieces[pos2.Item1, pos2.Item2];
            allMapPieces[pos2.Item1, pos2.Item2] = temp;

            //Set elements on inactive if in elemental region
            foreach(Transform child in allMapPieces[pos1.Item1, pos1.Item2].dungeonPart.transform)
            {
                ElementBehaviour elementBehaviour = child.GetComponent<ElementBehaviour>();
                if (elementBehaviour != null)
                {
                    child.gameObject.SetActive(elementBehaviour.ElementWeakness != dungeonAttributes.Elements[pos1.Item1, pos1.Item2]);
                }
            }
            foreach (Transform child in allMapPieces[pos2.Item1, pos2.Item2].dungeonPart.transform)
            {
                ElementBehaviour elementBehaviour = child.GetComponent<ElementBehaviour>();
                if (elementBehaviour != null)
                {
                    child.gameObject.SetActive(elementBehaviour.ElementWeakness != dungeonAttributes.Elements[pos2.Item1, pos2.Item2]);
                }
            }

            WaterScript[] oldSources = allSources.ToArray();
            foreach(WaterScript source in oldSources)
            {
                allSources.Remove(source);
                allSources.Add(source.TrackNewPath());
            }

            foreach(Transform child in piece1.dungeonPart.transform)
            {
                IDungeonSwapMessage reciever = child.GetComponent<IDungeonSwapMessage>();
                if(reciever != null)
                {
                    reciever.OnDungeonSwap();
                }
            }

            foreach (Transform child in piece2.dungeonPart.transform)
            {
                IDungeonSwapMessage reciever = child.GetComponent<IDungeonSwapMessage>();
                if (reciever != null)
                {
                    reciever.OnDungeonSwap();
                }
            }

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

    public MapPiece GetMapPiece(int x, int y)
    {
        if(x < 0 || y < 0 || x >= allMapPieces.GetLength(0) || y >= allMapPieces.GetLength(1))
        {
            Debug.LogWarning("x and y were outside of bounds. x is:" + x + ". y is:" + y);
            return null;
        }

        return allMapPieces[x, y];
    }

    public Element FindElementOfMapPiece(MapPiece mapPiece)
    {
        (int, int) pos = FindMapPiecePositionInArray(mapPiece);
        return DungeonAttributes.Current.GetElementOfArea(pos.Item1, pos.Item2);
    }

    public void PrepareForClosure()
    {
        foreach(MapPiece mapPiece in allMapPieces)
        {
            mapPiece.ResetWithoutSwap();
        }
    }
}
