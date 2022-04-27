using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerUITEST playerUI;

    void Start()
    {
        playerUI = GetComponent<PlayerUITEST>();
        maxHealth = 100;

        SetStats();
    }

    public void SetStats()
    {
        playerUI.attack.text = (Inventory.instance.GetAttackModifier()).ToString();
        playerUI.defense.text = defense.ToString();
        playerUI.health.text = currHealth.ToString();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        SetStats();
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.instance.KillPlayer();
    }
}
