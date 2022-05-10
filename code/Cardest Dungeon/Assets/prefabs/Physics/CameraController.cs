using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Shader waterShader;
    [SerializeField]
    private string replacementTag;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.SetReplacementShader(waterShader, replacementTag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
