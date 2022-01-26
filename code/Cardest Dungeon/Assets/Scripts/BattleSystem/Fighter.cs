using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public abstract class Fighter : MonoBehaviour
{
    /* The statemachine has the following states and transitions:
     * From     | Idle      | Attack    | Die
     * to       |
     * Idle:    |-------    |--------   |------
     * Attack:  |-------    |--------   |------
     * Die:     |-------    |--------   |------
     * */

    public enum Team
    {
        Player,
        Enemy
    }

    [System.Serializable]
    public struct Status
    {
        public int attack;
        public int defence;
        public int health;
    }

    protected StateMachine stateMachine;

    [SerializeField]
    protected Status status;

    [SerializeField]
    private Team team;

    private float restTimer = 0;
    private float restTime = 3; //How long the "ai" will pause between actions

    //DEBUG
    [SerializeField]
    private Team foe;

    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(restTimer);
        transform.localScale = new Vector3(status.health / 20f, status.health / 20f, 0);
        if (restTimer <= 0)
        {
            stateMachine.Run();
        }
        else
        {
            restTimer -= Time.deltaTime;
        }

        Debug.LogWarning(stateMachine.GetActiveStateName());
    }

    #region StateMachineStuff
    protected void InitializeStateMachine()
    {
        stateMachine = new StateMachine("Idle", IdleBody);
        stateMachine.AddState("Attack", AttackBody);
        stateMachine.AddState("Die", DieBody);

        stateMachine.AddTransition("Idle", "Attack", IdleToAttack);
        stateMachine.AddTransition("Idle", "Die", IdleToDie);
        stateMachine.AddTransition("Attack", "Idle", AttackToIdle);
    }

    private void IdleBody()
    {
        
    }

    private void AttackBody()
    {
        Attack();
        stateMachine.TransitionToState("Idle");
    }

    private void DieBody()
    {
        Destroy(gameObject);
    }

    private void IdleToAttack()
    {
        restTimer = restTime;
        Debug.LogError("Ich hab soger 'n Übergang gemacht");
    }

    private void AttackToIdle()
    {
        restTimer = restTime;
        EndTurn();
    }

    private void IdleToDie()
    {
        restTimer = restTime;
    }
    #endregion StateMachineStuff

    public void ActivateMyTurn()
    {
        stateMachine.TransitionToState("Attack");
        Debug.LogError("Ich wurde auf Attack gestellt. Hier ist der Beweis: " + stateMachine.GetActiveStateName());
    }

    public Team GetTeam()
    {
        return team;
    }

    public void Attack()
    {
        Fighter[] playerTeam = BattleMaster.Current.GetAllFightersFromTeam(foe);
        GetComponent<AudioSource>().Play();

        if (playerTeam.Length > 0)
        {
            BattleMaster.Current.AttackFighter(this, playerTeam[Random.Range(0, playerTeam.Length)]);
        }
    }

    public void GetAttacked(int attackDamage)
    {
        status.health -= Mathf.Max(attackDamage - status.defence, 0);
        
        if(status.health <= 0)
        {
            Debug.Log("Ja hallo, ich wurde gerade angegriffen und sollte jetzt eigentlich tot sein.");
            stateMachine.TransitionToState("Die");
        }
    }

    public Status GetStatus()
    {
        return status;
    }

    public void EndTurn()
    {
        BattleMaster.Current.EndTurn();
    }
}
