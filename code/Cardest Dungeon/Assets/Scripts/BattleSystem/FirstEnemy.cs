using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemy : Fighter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public new void Attack()
    {
        Fighter[] playerTeam = BattleMaster.Current.GetAllFightersFromTeam(Team.Player);

        BattleMaster.Current.AttackFighter(this, playerTeam[Random.Range(0, playerTeam.Length)]);
    }
}
