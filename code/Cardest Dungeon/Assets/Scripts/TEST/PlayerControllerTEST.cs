using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerTEST : MonoBehaviour {


    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float interactionRadius;

    private Rigidbody2D rig = default;
    private SpriteRenderer spriteRenderer;

    public Animator animator;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


/*    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
    }*/

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rig.velocity = InputManager.CalculateMovement() * runningSpeed;
        }
        else
        {
            rig.velocity = InputManager.CalculateMovement() * speed;
        }

        animator.SetFloat("a_Speed", rig.velocity.magnitude);

        if (rig.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rig.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKeyDown(InputManager.action))
        {
            Debug.Log("Aktionstaste gedrückt");

            List<Collider2D> results;
            results = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();

            Physics2D.OverlapCircle(gameObject.transform.position, interactionRadius, contactFilter.NoFilter(), results);
            foreach (Collider2D collider in results)
            {
                if (collider.tag.Equals("Interactable"))
                {
                    collider.GetComponent<ItemPickup>().Interact();
                    
                }
            }
        }
    }

}
