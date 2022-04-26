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
        currHealth = maxHealth;

        SetStats();
    }

    void SetStats()
    {
        playerUI.attack.text = attack.ToString();
        playerUI.defense.text = defense.ToString();
    }

}
