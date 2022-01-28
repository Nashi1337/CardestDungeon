using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public abstract class Fighter : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {

    }

    protected abstract void InitializeStateMachine();

    public abstract void ActivateMyTurn();

    public void Attack()
    {
        Fighter[] opponents = BattleMaster.Current.GetAllFightersFromTeam(foe);
        GetComponent<AudioSource>().Play();

        if (opponents.Length > 0)
        {
            BattleMaster.Current.AttackFighter(this, opponents[Random.Range(0, opponents.Length)], Random.Range(status.attack - 2, status.attack + 2));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackDamage"></param>
    /// <returns>Returns the actual damage taken.</returns>
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
    /// 
    /// </summary>
    /// <param name="health"></param>
    /// <returns>the actual amount of lives that were healed.</returns>
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

    public Team GetTeam()
    {
        return team;
    }

    public Status GetStatus()
    {
        return status;
    }

    public int GetLevel()
    {
        return level;
    }

    public void EndTurn()
    {
        BattleMaster.Current.EndTurn();
    }
}
