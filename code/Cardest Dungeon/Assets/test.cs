using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody r1;

    // Start is called before the first frame update
    void Start()
    {
        r1 = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("a"))
        {
            r1.AddForce(new Vector3(-1, 0, 0), ForceMode.Impulse);
        }
    }
}
