using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public class EnemyPhantom : MonoBehaviour
{
    //General
    [SerializeField]
    private float moveRadius;

    private float stateTimer;
    private Element mainState;
    private Vector3 origin;
    private Vector2 moveDirection;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriterenderer;
    private MapPiece homeArea;

    //Icebehaviour
    [SerializeField]
    private float ice_moveTime;
    [SerializeField]
    private float ice_idleTime;
    [SerializeField]
    private float ice_acceleration;

    private StateMachine ice_Behaviour;

    //FireBehaviour
    [SerializeField]
    private float fire_moveTime;
    [SerializeField]
    private float fire_idleTime;
    [SerializeField]
    private float fire_acceleration;
    [SerializeField]
    private float fire_sightRadius;
    [SerializeField]
    private float fire_retreatTime;

    private StateMachine fire_Behaviour;

    void Start()
    {
        origin = transform.position;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        homeArea = MapManager.Current.FindClosestMapPieceByPosition(transform.position);

        ice_Behaviour = new StateMachine("Init", Ice_Init);
        ice_Behaviour.AddState("Idle", Ice_Idling);
        ice_Behaviour.AddState("Move", Ice_Moving);
        ice_Behaviour.AddTransition("Idle", "Move", Ice_IdlingToMoving);
        ice_Behaviour.AddTransition("Move", "Idle", Ice_MovingToIdle);
        ice_Behaviour.AddTransition("Init", "Idle", Ice_MovingToIdle);

        fire_Behaviour = new StateMachine("Init", Fire_Init);
        fire_Behaviour.AddState("Idle", Fire_Idling);
        fire_Behaviour.AddState("Move", Fire_Moving);
        fire_Behaviour.AddState("AttackPlayer", Fire_Attacking);
        fire_Behaviour.AddState("RetreatFromPlayer", Fire_Retreating);
        fire_Behaviour.AddTransition("Idle", "Move", Fire_IdlingToMoving);
        fire_Behaviour.AddTransition("Move", "Idle", Fire_MovingToIdle);
        fire_Behaviour.AddTransition("Init", "Idle", Fire_MovingToIdle);
        fire_Behaviour.AddTransition("AttackPlayer", "RetreatFromPlayer", Fire_ToRetreat);
        fire_Behaviour.AddTransition("RetreatFromPlayer", "Idle", Fire_ToIdle);

        UpdateMainState();

        Debug.LogWarning("Sprite flippen und moveDirection ändern in eine Methode packen");
    }

    //This empty method is necessary in order to overwrite the methods from Enemy
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(stateTimer >= 0)
        {
            stateTimer -= Time.fixedDeltaTime;
        }

        switch(mainState)
        {
            case Element.ICE:
                ice_Behaviour.Run();
                break;
            case Element.FIRE:
                Debug.Log(fire_Behaviour.GetActiveStateName());
                fire_Behaviour.Run();

                string activeState = fire_Behaviour.GetActiveStateName();
                if (!activeState.Equals("AttackPlayer")
                    && !activeState.Equals("RetreatFromPlayer")
                    && IsPlayerInSight()
                    )
                {
                    Debug.LogWarning("Attack");
                    fire_Behaviour.TransitionToState("AttackPlayer");
                }
                break;
        }

    }

    private void UpdateMainState()
    {
        mainState = MapManager.Current.FindElementOfMapPiece(homeArea);

        switch(mainState)
        {
            case Element.ICE:
                animator.speed = 0.6f;
                break;
            case Element.FIRE:
                animator.speed = 1.3f;
                break;
            case Element.NONE:
                animator.speed = 1f;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mainState == Element.FIRE && collision.gameObject.tag.Equals("Player"))
        {
            Debug.LogWarning("Retreat from Player should probaly happen in another place. Recheck this after adding enemy behaviour to Phantom.");
            fire_Behaviour.TransitionToState("RetreatFromPlayer");
        }
    }

    #region IceBehaviour
    private void Ice_Init()
    {
        ice_Behaviour.TransitionToState("Idle");
    }

    private void Ice_Idling()
    {
        if(stateTimer <= 0)
        {
            ice_Behaviour.TransitionToState("Move");
        }
    }

    private void Ice_Moving()
    {
        rb.AddForce(moveDirection * ice_acceleration);

        if (stateTimer <= 0)
        {
            ice_Behaviour.TransitionToState("Idle");
        }
    }

    private void Ice_IdlingToMoving()
    {
        animator.SetBool("isMoving", true);
        if ((origin - transform.position).magnitude < moveRadius)
        {
            moveDirection.x = Random.Range(-1f, 1f);
            moveDirection.y = Random.Range(-1f, 1f);
            moveDirection.Normalize();
        }
        else
        {
            moveDirection *= -1;
        }
        spriterenderer.flipX = moveDirection.x < 0;

        stateTimer = Random.Range(ice_moveTime * 0.8f, ice_moveTime * 1.2f);
    }

    private void Ice_MovingToIdle()
    {
        animator.SetBool("isMoving", false);

        stateTimer = Random.Range(ice_idleTime * 0.8f, ice_idleTime * 1.2f);
    }
    #endregion

    #region FireBehaviour
    private void Fire_Init()
    {
        fire_Behaviour.TransitionToState("Idle");
    }

    private void Fire_Idling()
    {
        if(stateTimer <= 0)
        {
            fire_Behaviour.TransitionToState("Move");
        }
    }

    private void Fire_Moving()
    {
        rb.AddForce(moveDirection * fire_acceleration);

        if(stateTimer <= 0)
        {
            fire_Behaviour.TransitionToState("Idle");
        }
    }

    private void Fire_Attacking()
    {
        moveDirection = (PlayerController.Current.transform.position - transform.position).normalized;
        rb.AddForce(moveDirection * fire_acceleration);
        spriterenderer.flipX = moveDirection.x < 0;
    }

    private void Fire_Retreating()
    {
        rb.AddForce(moveDirection * fire_acceleration * 0.5f);

        if(stateTimer <= 0)
        {
            fire_Behaviour.TransitionToState("Idle");
        }
    }

    private void Fire_IdlingToMoving()
    {
        animator.SetBool("isMoving", true);
        if ((origin - transform.position).magnitude < moveRadius)
        {
            moveDirection.x = Random.Range(-1f, 1f);
            moveDirection.y = Random.Range(-1f, 1f);
            moveDirection.Normalize();
        }
        else
        {
            moveDirection *= -1;
        }
        spriterenderer.flipX = moveDirection.x < 0;

        stateTimer = Random.Range(fire_moveTime * 0.8f, fire_moveTime * 1.2f);
    }

    private void Fire_MovingToIdle()
    {
        animator.SetBool("isMoving", false);

        Fire_ToIdle();
    }

    private void Fire_ToIdle()
    {
        Debug.Log("Ich wurde gerufen");

        stateTimer = Random.Range(fire_idleTime * 0.8f, fire_idleTime * 1.2f);
    }

    private void Fire_ToRetreat()
    {
        moveDirection = -(PlayerController.Current.transform.position - transform.position).normalized;
        spriterenderer.flipX = moveDirection.x < 0;

        stateTimer = fire_retreatTime;
    }
    #endregion

    private bool IsPlayerInSight()
    {
        return (PlayerController.Current.transform.position - transform.position).magnitude < fire_sightRadius;
    }
}
