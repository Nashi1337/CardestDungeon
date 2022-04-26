using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

/// <summary>
/// The most basic and general class for the turn based battle. Every entity which wants to participate needs to inherit from this class.
/// The Fighter class delivers the most essential and obligatory elements that are needed in order to take part in a battle.
/// </summary>
public abstract class Fighter : MonoBehaviour
{
    /// <summary>
    /// Represents the team in which a fighter is in
    /// </summary>
    public enum Team
    {
        Player,
        Enemy
    }

    /// <summary>
    /// Represents all status points a fighter has
    /// </summary>
    [System.Serializable]
    public struct Status
    {
        public int attack;
        public int defence;
        public int health;
    }

    protected StateMachine stateMachine;

    protected int maxHealth;
    [SerializeField]
    protected Status status;

    [SerializeField]
    private int level;
    [SerializeField]
    private Team team;
    [SerializeField]
    public BattleHUD BattleHUD { get; set; }

    //DEBUG
    [SerializeField]
    public Team foe;

    // Start is called before the first frame update
    void OnEnable()
    {
        maxHealth = status.health;
        InitializeStateMachine();
    }

    /// <summary>
    /// Obligates every derived class to create a statemachine which controls the behaviour of instances of the derived class. 
    /// </summary>
    protected abstract void InitializeStateMachine();

    /// <summary>
    /// Obligates every derived class to be able to have its turn being enabled.
    /// </summary>
    public abstract void ActivateMyTurn();

    /// <summary>
    /// Picks a random fighter from the opponent team, attacks it and plays an attack audio. If there are no opponents nothing will happen
    /// </summary>
    public void Attack()
    {
        Fighter[] opponents = BattleMaster.Current.FindAllFightersFromTeam(foe);

        if (opponents.Length > 0)
        {
            GetComponent<AudioSource>().Play();
            BattleMaster.Current.AttackFighter(this, opponents[Random.Range(0, opponents.Length)], Random.Range(status.attack - 2, status.attack + 2));
        }
    }

    /// <summary>
    /// Calculates the actual damage, subtracts it from the health and updates the fighter's HUD. 
    /// If health is below zero it destroys itself.
    /// </summary>
    /// <param name="attackDamage"></param>
    /// <returns></returns>
    public int GetAttacked(int attackDamage)
    {
        int actualDamage = Mathf.Max(attackDamage - status.defence, 0);
        status.health -= actualDamage;
        BattleHUD.SetHealth(status.health);
        if (status.health <= 0)
        {
            stateMachine.TransitionToState("Die");
        }
        return actualDamage;
    }

    /// <summary>
    /// Heals health amount of health points or fully. Depending on which is less.
    /// </summary>
    /// <param name="health">The amount of health points that should be healed.</param>
    /// <returns>How many health points were actually healed.</returns>
    public int GetHealed(int health)
    {
        if(status.health + health > maxHealth)
        {
            health = maxHealth - status.health;
        }

        status.health += health;
        BattleHUD.SetHealth(status.health);

        return health;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the team of this fighter.</returns>
    public Team GetTeam()
    {
        return team;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the current stats of this fighter.</returns>
    public Status GetStatus()
    {
        return status;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the level of this fighter.</returns>
    public int GetLevel()
    {
        return level;
    }

    /// <summary>
    /// Tells the BattleMaster that the turn of this fighter has ended.
    /// </summary>
    public void EndTurn()
    {
        BattleMaster.Current.EndTurn();
    }
}
