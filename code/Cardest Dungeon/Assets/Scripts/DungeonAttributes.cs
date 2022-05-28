using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAttributes : MonoBehaviour
{
    public int DungeonWidth { get { return dungeonWidth; } }
    public int DungeonHeight { get { return dungeonHeight; } }

    public Element[,] Elements
    {
        get
        {
            if (elements2DArray == null)
            {
                elements2DArray = new Element[dungeonWidth, dungeonHeight];
                int index = 0;
                for (int j = 0; j < elements2DArray.GetLength(1); j++)
                {
                    for (int i = 0; i < elements2DArray.GetLength(0); i++)
                    {
                        elements2DArray[i, j] = elementsLeftToRightTopToBottom[index];
                        index++;
                        if (index >= elementsLeftToRightTopToBottom.Length)
                        {
                            Debug.LogWarning("DungeonWidth * DungeonHeight does not match elements.Length");
                            return elements2DArray;
                        }
                    }
                }
            }
            return elements2DArray;
        }
    }

    [SerializeField]
    private int dungeonWidth;
    [SerializeField]
    private int dungeonHeight;
    [SerializeField]
    private Element[] elementsLeftToRightTopToBottom;

    private Element[,] elements2DArray;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
