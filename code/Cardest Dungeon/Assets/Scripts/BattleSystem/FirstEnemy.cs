using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemy : Enemy
{
    public new void Attack()
    {
        Fighter[] playerTeam = BattleMaster.Current.GetAllFightersFromTeam(foe);
        GetComponent<AudioSource>().Play();

        if (playerTeam.Length > 0)
        {
            BattleMaster.Current.AttackFighter(this, playerTeam[Random.Range(0, playerTeam.Length)], Random.Range(status.attack - 2, status.attack + 2));
        }
    }
}
