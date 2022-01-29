using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

/// <summary>
/// Represents a fighter that can be controlled by the player through the GUI
/// </summary>
public class PlayerFighter : Fighter
{
    // Update is called once per frame
    void Update()
    {
        stateMachine.Run();
    }

    #region StateMachineStuff
    protected override void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Idle", IdleBody); //As long as it's not the player's turn
        stateMachine.AddState("SelectMove", SelectMoveBody); //As long as it's the player's turn and he didn't choose an option, yet.
        stateMachine.AddState("SelectEnemy", SelectEnemyBody); //As long as player is selecting an enemy
        stateMachine.AddState("Attack", AttackBody); //As long as player is attacking
        stateMachine.AddState("Die", DieBody);      //You dead bro

        stateMachine.AddTransition("Idle", "SelectMove", IdleToSelectMove);
        stateMachine.AddTransition("Idle", "Die", ToDie);

        stateMachine.AddTransition("SelectMove", "SelectEnemy", SelectMoveToSelectEnemy);
        stateMachine.AddTransition("SelectMove", "Die", ToDie);

        stateMachine.AddTransition("SelectEnemy", "SelectMove", SelectEnemyToSelectMove);
        stateMachine.AddTransition("SelectEnemy", "Attack", SelectEnemyToAttack);
        stateMachine.AddTransition("SelectEnemy", "Die", ToDie);

        stateMachine.AddTransition("Attack", "Idle", AttackToIdle);
        stateMachine.AddTransition("Attack", "Die", ToDie);
    }

    protected void IdleBody()
    {

    }

    protected void SelectMoveBody()
    {

    }

    protected void SelectEnemyBody()
    {

    }

    protected void AttackBody()
    {
        Attack();
        stateMachine.TransitionToState("Idle");
    }

    protected void DieBody()
    {

    }

    private void IdleToSelectMove()
    {
        BattleMaster.Current.ActivatePlayerActions();
        BattleMaster.Current.WriteToDialogue("Choose an action: ");
    }

    private void SelectMoveToSelectEnemy()
    {
        //Hier wird der Pfeil aktiviert.
    }

    private void SelectEnemyToSelectMove()
    {
        //Hier wird der Pfeil deaktiviert.
    }

    private void SelectEnemyToAttack()
    {
        //Hier wird der Pfeil deaktiviert und angegriffen

    }

    private void AttackToIdle()
    {
        //Hier wird die Kontrolle zurück an den BattleMaster gegeben.
        BattleMaster.Current.DeactivatePlayerActions();
        EndTurn();
    }

    private void ToDie()
    {

    }
    #endregion StateMachineStuff

    public override void ActivateMyTurn()
    {
        stateMachine.TransitionToState("SelectMove");
    }

    /// <summary>
    /// Will be executed when the player presses on the attack button
    /// </summary>
    public void OnAttackButton()
    {
        if (stateMachine.GetActiveStateName() == "SelectMove")
        {
            stateMachine.TransitionToState("SelectEnemy");
            stateMachine.TransitionToState("Attack");
        }
    }

    /// <summary>
    /// Will be executed when the player presses on the attack button
    /// </summary>
    public void OnHealButton()
    {
        if (stateMachine.GetActiveStateName() == "SelectMove")
        {
            //Das hier bitte sauberer machen
            BattleMaster.Current.HealFighter(this, 7);
            stateMachine.TransitionToState("Idle");
            
            //Das hier bitte sauberer machen
            BattleMaster.Current.DeactivatePlayerActions();
            EndTurn();
        }
    }
}
