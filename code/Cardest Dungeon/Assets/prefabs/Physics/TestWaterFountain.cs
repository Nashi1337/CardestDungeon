using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterFountain : MonoBehaviour
{
    public GameObject water;
    public float spawnRate;


    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= spawnRate)
        {
            Debug.Log("Erstellt");
            timer -= spawnRate;
            Instantiate(water, transform.position + new Vector3(0, 0, 10), Quaternion.identity);
        }
        timer += Time.deltaTime;
    }
}
