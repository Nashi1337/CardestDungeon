using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the direction of the water and splits it if more than one direction is specified.
/// If none are specified the water will be stopped.
/// </summary>
public class WaterControlPoint : MonoBehaviour
{

    public Vector2[] FlowDirections
    {
        get
        {
            return flowDirections;
        }
        set
        {
            flowDirections = NormalizeAllVectors(value);
        }
    }
    [SerializeField]
    public bool StopsWater
    {
        get
        {
            return stopsWater;
        }
        set
        {
            stopsWater = value;
        }
    }
    
    [SerializeField]
    private Vector2[] flowDirections;
    [SerializeField]
    private bool stopsWater;

    private void Start()
    {
        NormalizeAllVectors(FlowDirections);
    }


    /// <summary>
    /// Normalizes every vector in the array.
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns>The normalized vectors in the original order.</returns>
    private Vector2[] NormalizeAllVectors(Vector2[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
            vectors[i].Normalize();

        return vectors;
    }
}