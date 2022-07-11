using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
/*    [SerializeField]
    private Text enemyattack;
    [SerializeField]
    private Text enemydefense;
    [SerializeField]
    private Text enemyhealth;*/

    void Start()
    {
        UpdateStats();
        Initialize();
    }

    public override void UpdateStats()
    {
/*        enemyattack.text = Attack.ToString();
        enemydefense.text = Defense.ToString();
        enemyhealth.text = CurrHealth.ToString();*/
        base.UpdateStats();
    }

    public override int TakeDamage(int attackValue)
    {
        return base.TakeDamage(attackValue, Defense);
    }
}
