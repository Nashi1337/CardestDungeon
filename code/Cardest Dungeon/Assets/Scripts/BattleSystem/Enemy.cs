using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public abstract class Enemy : Fighter
{

    protected float restTimer = 0;
    protected float restTime = 0.2f; //How long the "ai" will pause between actions
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(status.health / (float)maxHealth, status.health / (float)maxHealth, 0);
        if (restTimer <= 0)
        {
            stateMachine.Run();
        }
        else
        {
            restTimer -= Time.deltaTime;
        }
    }

    #region StateMachineStuff
    protected override void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Idle", IdleBody);
        stateMachine.AddState("Attack", AttackBody);
        stateMachine.AddState("Die", DieBody);

        stateMachine.AddTransition("Idle", "Attack", IdleToAttack);
        stateMachine.AddTransition("Attack", "Idle", AttackToIdle);
        stateMachine.AddTransition("Idle", "Die", ToDie);
        stateMachine.AddTransition("Attack", "Die", ToDie);
    }

    protected virtual void IdleBody()
    {

    }

    protected virtual void AttackBody()
    {
        Attack();
        stateMachine.TransitionToState("Idle");
    }

    protected virtual void DieBody()
    {

    }

    protected virtual void IdleToAttack()
    {
        restTimer = restTime;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    protected virtual void AttackToIdle()
    {
        restTimer = restTime;
        GetComponent<SpriteRenderer>().color = Color.white;
        EndTurn();
    }

    protected virtual void ToDie()
    {
        BattleMaster.Current.DestroyFighter(this);
    }
    #endregion StateMachineStuff

    public override void ActivateMyTurn()
    {
        stateMachine.TransitionToState("Attack");
    }
}
