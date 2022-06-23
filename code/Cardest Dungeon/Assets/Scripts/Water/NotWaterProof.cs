using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotWaterProof : MonoBehaviour
{

    void Start()
    {
        
    }

 
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        Debug.Log("Ich bin " + this.name + " und bin mit " + collision.gameObject.name + " kollidiert");
        if(collision.gameObject.GetComponent<WaterScript>() != null)
        {
            if(collision.gameObject.GetComponent<WaterScript>().isFlowing > 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
