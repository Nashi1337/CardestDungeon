using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static DefaultInputActions Current
    {
        get
        {
            if(current == null)
            {
                current = new DefaultInputActions();
                current.Enable();
            }
            return current;
        }
    }

    private static DefaultInputActions current = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
