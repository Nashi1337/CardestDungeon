using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;
using System;

public class EnemyPhantom : Enemy
{
    [Serializable]
    struct Behaviour
    {
        public float moveTime;
        public float idleTime;
        public float acceleration;
        public float sightRadius;
        public float retreatTime;
    }

    [Serializable]
    struct Stats
    {
        public int maxHealth;
        public int attack;
        public int defense;
        public int magic;
    }

    [SerializeField]
    private float moveRadius;
    [SerializeField]
    private Behaviour ice_Behaviour;
    [SerializeField]
    private Behaviour fire_Behaviour;
    [SerializeField]
    private Behaviour normal_Behaviour;

    [SerializeField]
    private Stats ice_Stats;
    [SerializeField]
    private Stats fire_Stats;
    [SerializeField]
    private Stats normal_Stats;

    private float stateTimer;
    private Element mainState;
    private Behaviour active_Behaviour;
    private Vector3 origin;
    private Vector2 moveDirection;
    private MapPiece homeArea;
    private StateMachine stateMachine;

    void Start()
    {
        Initialize();

        origin = transform.position;

        homeArea = MapManager.Current.FindClosestMapPieceByPosition(transform.position);

        stateMachine = new StateMachine("Init", Fire_Init);
        stateMachine.AddState("Idle", Fire_Idling);
        stateMachine.AddState("Move", Fire_Moving);
        stateMachine.AddState("AttackPlayer", Fire_Attacking);
        stateMachine.AddState("RetreatFromPlayer", Fire_Retreating);
        stateMachine.AddTransition("Idle", "Move", Fire_IdlingToMoving);
        stateMachine.AddTransition("Move", "Idle", Fire_MovingToIdle);
        stateMachine.AddTransition("Init", "Idle", Fire_MovingToIdle);
        stateMachine.AddTransition("AttackPlayer", "RetreatFromPlayer", Fire_ToRetreat);
        stateMachine.AddTransition("RetreatFromPlayer", "Idle", Fire_ToIdle);

        ApplyStatsToEnemyStats(normal_Stats);
        active_Behaviour =normal_Behaviour;

        UpdateMainState();

    }

    //This empty method is necessary in order to overwrite the methods from Enemy
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (stateTimer >= 0)
        {
            stateTimer -= Time.fixedDeltaTime;
        }

        stateMachine.Run();

        string activeState = stateMachine.GetActiveStateName();
        if (!activeState.Equals("AttackPlayer")
            && !activeState.Equals("RetreatFromPlayer")
            && IsPlayerInSight()
            )
        {
            stateMachine.TransitionToState("AttackPlayer");
        }
    }

    public void UpdateMainState()
    {
        Element newState = MapManager.Current.FindElementOfMapPiece(homeArea);
        if(newState == mainState)
        {
            return;
        }
        mainState = newState;

        switch(mainState)
        {
            case Element.ICE:
                animator.speed = 0.6f;
                SwitchBehaviour(ice_Behaviour);
                animator.SetBool("isRed", false);
                ApplyStatsToEnemyStats(ice_Stats);
                break;
            case Element.FIRE:
                animator.speed = 1.3f;
                SwitchBehaviour(fire_Behaviour);
                animator.SetBool("isRed", true);
                ApplyStatsToEnemyStats(fire_Stats);
                break;
            case Element.NONE:
                animator.speed = 1f;
                SwitchBehaviour(normal_Behaviour);
                animator.SetBool("isRed", false);
                ApplyStatsToEnemyStats(normal_Stats);
                break;
        }

        enemyStats.UpdateMaxHealth();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mainState != Element.ICE && stateMachine.GetActiveStateName().Equals("AttackPlayer") && collision.gameObject.tag.Equals("Player"))
        {
            stateMachine.TransitionToState("RetreatFromPlayer");
        }
    }

    #region StateMachine
    private void Fire_Init()
    {
        stateMachine.TransitionToState("Idle");
    }

    private void Fire_Idling()
    {
        if(stateTimer <= 0)
        {
            stateMachine.TransitionToState("Move");
        }
    }

    private void Fire_Moving()
    {
        rb.AddForce(moveDirection * active_Behaviour.acceleration);

        if(stateTimer <= 0)
        {
            stateMachine.TransitionToState("Idle");
        }
    }

    private void Fire_Attacking()
    {
        moveDirection = (PlayerController.Current.transform.position - transform.position).normalized;
        rb.AddForce(moveDirection * active_Behaviour.acceleration);
        spriterenderer.flipX = moveDirection.x < 0;
    }

    private void Fire_Retreating()
    {
        rb.AddForce(moveDirection * active_Behaviour.acceleration * 0.5f);

        if(stateTimer <= 0)
        {
            stateMachine.TransitionToState("Idle");
        }
    }

    private void Fire_IdlingToMoving()
    {
        animator.SetBool("isMoving", true);
        if ((origin - transform.position).magnitude < moveRadius)
        {
            moveDirection.x = UnityEngine.Random.Range(-1f, 1f);
            moveDirection.y = UnityEngine.Random.Range(-1f, 1f);
            moveDirection.Normalize();
        }
        else
        {
            moveDirection *= -1;
        }
        spriterenderer.flipX = moveDirection.x < 0;

        UpdateMainState();

        stateTimer = UnityEngine.Random.Range(active_Behaviour.moveTime * 0.8f, active_Behaviour.moveTime * 1.2f);
    }

    private void Fire_MovingToIdle()
    {
        animator.SetBool("isMoving", false);

        UpdateMainState();

        Fire_ToIdle();
    }

    private void Fire_ToIdle()
    {
        UpdateMainState();

        stateTimer = UnityEngine.Random.Range(active_Behaviour.idleTime * 0.8f, active_Behaviour.idleTime * 1.2f);
    }

    private void Fire_ToRetreat()
    {
        moveDirection = -(PlayerController.Current.transform.position - transform.position).normalized;
        spriterenderer.flipX = moveDirection.x < 0;

        stateTimer = active_Behaviour.retreatTime;
    }
    #endregion

    private bool IsPlayerInSight()
    {
        return (PlayerController.Current.transform.position - transform.position).magnitude < active_Behaviour.sightRadius;
    }

    private void SwitchBehaviour(Behaviour newBehaviour)
    {
        active_Behaviour = newBehaviour;
    }

    private void ApplyStatsToEnemyStats(Stats stats)
    {
        enemyStats.MaxHealth = stats.maxHealth;
        enemyStats.Attack = stats.attack;
        enemyStats.Defense = stats.defense;
        enemyStats.Magic = stats.magic;

        enemyStats.Heal(stats.maxHealth);
        UpdateScaleToStats();
    }

    private void UpdateScaleToStats()
    {
        float scaleModifier = 0.4f;
        scaleModifier += enemyStats.Defense / 15f;
        if (enemyStats.Magic != 0)
        {
            scaleModifier += enemyStats.Attack / 45f;
            scaleModifier += enemyStats.Magic / 15f;
        }
        else
        {
            scaleModifier += enemyStats.Magic / 30f;
            scaleModifier += enemyStats.Attack / 15f;
        }
        transform.localScale = Vector3.one * scaleModifier;
    }
}
