using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public class PlayerFighter : Fighter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //Hier werden die Kn�pfe freigeschaltet.
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
        //Hier wird die Kontroller zur�ck an den BattleMaster gegeben.
    }

    private void ToDie()
    {

    }

    //protected void IdleToAttack()
    //{
    //    GetComponent<SpriteRenderer>().color = Color.blue;
    //}

    //protected void AttackToIdle()
    //{
    //    GetComponent<SpriteRenderer>().color = Color.white;
    //    EndTurn();
    //}

    //protected void ToDie()
    //{
    //    BattleMaster.Current.DestroyFighter(this);
    //}
    #endregion StateMachineStuff

    public override void ActivateMyTurn()
    {
        stateMachine.TransitionToState("SelectMove");
    }
}
