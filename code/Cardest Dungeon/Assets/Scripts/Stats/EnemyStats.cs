using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    EnemyUITEST enemyUI;

    void Start()
    {
        enemyUI = GetComponent<EnemyUITEST>();
        Debug.Log("Hallo");
        SetStats();
    }

    public void SetStats()
    {
        //Debug.Log("Servus");
        //Debug.Log("attack: " + attack + ", defense: " + defense + ", currentHealth: "+ CurrHealth);
        enemyUI.enemyattack.text = Attack.ToString();
        enemyUI.enemydefense.text = Defense.ToString();
        enemyUI.enemyhealth.text = CurrHealth.ToString();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        SetStats();
    }

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
