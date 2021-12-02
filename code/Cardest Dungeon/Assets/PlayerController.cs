using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    private Rigidbody2D rig = default;

    private SpriteRenderer spriterRenderer;

    public AudioClip test;

    //animator Variable um für den Player Animationen zu steuern
    public Animator animator;

    private GameObject mapeditor;
    // Start is called before the first frame update

    Transform[] allchildren;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        mapeditor = GameObject.Find("MapEditor");
        allchildren = mapeditor.GetComponentsInChildren<Transform>();
        spriterRenderer = GetComponent < SpriteRenderer>();

        //MapEditor and it's children are set false at the start of the game so that the M button action works
        mapeditor.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rig.velocity = InputManager.CalculateMovement() * runningSpeed;
        }
        else
        {
            rig.velocity = InputManager.CalculateMovement() * speed;
        }
        //Der Parameter a_Speed ist wichtig für die Animation, bei einem Wert > 0.01 wird die walking Animation getriggert
        animator.SetFloat("a_Speed", rig.velocity.magnitude);

        if(rig.velocity.x < 0)
        {
            spriterRenderer.flipX = true;
        }
        else if(rig.velocity.x > 0)
        {
            spriterRenderer.flipX = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.map))
        {
            ShowHideMap();
        }
    }

    private void ShowHideMap()
    {
        mapeditor.SetActive(!mapeditor.activeSelf);

        foreach (Transform child in allchildren)
        {
            child.gameObject.SetActive(mapeditor.activeSelf);
        }
    }
}
