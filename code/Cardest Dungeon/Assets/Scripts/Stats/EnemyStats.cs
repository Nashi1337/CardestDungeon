using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{

    void Start()
    {
        UpdateStats();
        Initialize();
    }

    public override void UpdateStats()
    {
        base.UpdateStats();
    }

    public override int TakeDamage(int attackValue, CharacterStats attacker)
    {
        return base.TakeDamage(attackValue, Defense, attacker);
    }
}
