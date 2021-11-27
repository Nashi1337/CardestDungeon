using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rig = default;

    public AudioClip test;

    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        InputManager.Current.onMove += OnMove;
        InputManager.Current.onAction1Down += OnAction1Down;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMove(Vector2 input)
    {
        rig.velocity = input * speed;
    }

    private void OnAction1Down()
    {
        Debug.Log("Action1 wurde gedrückt");

        var audio = gameObject.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
            audio.clip = test;
        }
        audio.Play();

    }
}
