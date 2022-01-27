using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public abstract class Enemy : Fighter
{

    protected float restTimer = 0;
    protected float restTime = 2f; //How long the "ai" will pause between actions
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
        stateMachine.AddState("EndTurnWait", EndTurnWaitBody);


        stateMachine.AddTransition("Idle", "Attack", IdleToAttack);
        stateMachine.AddTransition("Attack", "Idle", AttackToIdle);
        stateMachine.AddTransition("Attack", "EndTurnWait", AttackToEndTurnWait);
        stateMachine.AddTransition("Idle", "Die", ToDie);
        stateMachine.AddTransition("Attack", "Die", ToDie);
    }

    protected virtual void IdleBody()
    {

    }

    protected virtual void AttackBody()
    {
        Attack();
        stateMachine.TransitionToState("EndTurnWait");
    }

    protected virtual void DieBody()
    {

    }

    protected virtual void EndTurnWaitBody()
    {
        stateMachine.TransitionToState("Idle");
        EndTurn();
    }

    protected virtual void IdleToAttack()
    {
        restTimer = restTime;
    }

    protected virtual void AttackToIdle()
    {
        restTimer = restTime;
    }

    protected virtual void AttackToEndTurnWait()
    {
        restTimer = restTime;
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
