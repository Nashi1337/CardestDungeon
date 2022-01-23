using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    protected Status status;
    [SerializeField]
    private Team team;

    private bool myTurn = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMyTurn()
    {
        myTurn = true;
    }

    public Team GetTeam()
    {
        return team;
    }

    public void Attack()
    {
        Fighter[] playerTeam = BattleMaster.Current.GetAllFightersFromTeam(Team.Player);

        BattleMaster.Current.AttackFighter(this, playerTeam[Random.Range(0, playerTeam.Length)]);
    }

    public void GetAttacked(int attackDamage)
    {
        status.health -= Mathf.Max(attackDamage - status.defence, 0);

        if(status.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Status GetStatus()
    {
        return status;
    }

    public void EndTurn()
    {
        BattleMaster.Current.EndTurn();
        myTurn = false;
    }
}
