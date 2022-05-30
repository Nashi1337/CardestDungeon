using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotWaterProof : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ich bin " + this.name + " und bin mit " + collision.gameObject.name + " kollidiert");
        if(collision.gameObject.GetComponent<WaterScript>() != null)
        {
            Destroy(gameObject);
        }
    }
}
