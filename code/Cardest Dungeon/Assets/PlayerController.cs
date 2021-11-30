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

    public GameObject mapeditor;
    // Start is called before the first frame update

    Transform[] allchildren;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        mapeditor = GameObject.Find("MapEditor");
        allchildren = mapeditor.GetComponentsInChildren<Transform>();

        //MapEditor and it's children are set false at the start of the game so that the M button action works
        mapeditor.SetActive(false);
        foreach (Transform child in allchildren)
        {
            child.gameObject.SetActive(mapeditor.activeSelf);
            Debug.Log(child);
        }

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
        mapeditor.SetActive(!mapeditor.activeSelf);

        foreach (Transform child in allchildren)
        {
            child.gameObject.SetActive(mapeditor.activeSelf);
            Debug.Log(child);
        }

    }

}
