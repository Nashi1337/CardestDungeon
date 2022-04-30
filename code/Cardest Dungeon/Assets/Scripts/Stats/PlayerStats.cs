using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [SerializeField]
    private Text attackText;
    [SerializeField]
    private Text defenseText;
    [SerializeField]
    private Text healthText;

    void Start()
    {
        Initialize();
    }

    //Do not Use this except if you are the PlayerController. Else use either the SetStats or the UpdateStats of player controller
    public override void UpdateStats()
    {
        attackText.text = (Attack + Inventory.instance.GetAttackModifier()).ToString();
        defenseText.text = (Defense + Inventory.instance.GetDefenseModifier()).ToString();
        healthText.text = CurrHealth.ToString();
        base.UpdateStats();
    }

    public override int TakeDamage(int attackValue)
    {
        return base.TakeDamage(attackValue, Defense + Inventory.instance.GetDefenseModifier());
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
    }

    public override void Die()
    {
        base.Die();
        Debug.LogWarning("Du bist tot. Hier sollte jetzt etwas passieren.");
    }
}
