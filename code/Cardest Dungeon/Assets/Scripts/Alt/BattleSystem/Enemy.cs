using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

/// <summary>
/// This class should be used as a parent class for all other ai-controlled fighter-classes because it implements
/// basic behaviour all enemies should follow.
/// </summary>
public abstract class Enemy : Fighter
{

    protected float restTimer = 0;
    protected float restTime = 2f; //How long the "ai" will pause between actions

    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(status.health / (float)maxHealth * originalScale.x, status.health / (float)maxHealth * originalScale.y, originalScale.z);
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

    /// <summary>
    /// Creates the statemachine and adds all states and transitions to it. New states and transitions should all be added in this method.
    /// </summary>
    protected override void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Idle", IdleBody);
        stateMachine.AddState("Attack", AttackBody);
        stateMachine.AddState("Die", DieBody);
        stateMachine.AddState("EndTurnWait", EndTurnWaitBody); //Needed in order to wait 2 seconds before ending turn.

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

    /// <summary>
    /// Activates the statemachine of this enemy by setting the state to "Attack".
    /// </summary>
    public override void ActivateMyTurn()
    {
        stateMachine.TransitionToState("Attack");
    }
}
