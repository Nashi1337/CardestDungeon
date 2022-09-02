using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this script to an object in order to let it destroy itself when it touches an Object
/// with the WaterScript.
/// </summary>
public class NotWaterProof : MonoBehaviour
{
    /// <summary>
    /// certain objects will be destroyed when touching water
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<WaterScript>() != null)
        {
            if(collision.gameObject.GetComponent<WaterScript>().isFlowing > 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
