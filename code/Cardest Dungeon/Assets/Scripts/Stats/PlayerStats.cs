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
    [SerializeField]
    private Text magicText;

    public GameObject gameOver;

    new private AudioSource audio;

    void Start()
    {
        Initialize();
        audio = GetComponent<AudioSource>();
        //gameOver = FindObjectOfType<GameOver>().gameObject.GetComponent<GameOver>();
    }

    //Do not Use this except if you are the PlayerController. Else use either the SetStats or the UpdateStats of player controller
    public override void UpdateStats()
    {
        attackText.text = (Attack + Inventory.instance.GetAttackModifier()).ToString();
        defenseText.text = (Defense + Inventory.instance.GetDefenseModifier()).ToString();
        magicText.text = (Magic + Inventory.instance.GetMagicModifier()).ToString();
        healthText.text = CurrHealth.ToString();
        base.UpdateStats();
    }

    public override int TakeDamage(int attackValue)
    {
        if (attackValue > Defense)
        {
            audio.Play();
        }
        if (CurrHealth <= 0)
        {
            Die();
        }
        return base.TakeDamage(attackValue, Defense + Inventory.instance.GetDefenseModifier());
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
    }

    public static void Die()
    {
        //base.Die();
        Debug.Log("Du bist tot. Hier sollte jetzt etwas passieren.");
        PlayerController.canMove = false;
        PlayerController.Die();
    }
}
