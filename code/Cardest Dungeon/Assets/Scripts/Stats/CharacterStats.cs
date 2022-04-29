using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int currHealth { get; private set; }
    public int maxHealth;

    public HealthBar healthBar;

    public int attack;
    public int defense;

    public bool isDead = false;

    private void Awake()
    {
        currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(Mathf.Max(10 - Inventory.instance.GetDefenseModifier(),0));
        }    
    }

    public void TakeDamage(int damage)
    {
        damage -= defense;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currHealth -= damage;

        healthBar.SetHealth(currHealth);

        Debug.Log(transform.name + " takes " + damage + " damage. servus");

        CheckHealth();

    }

    public virtual void CheckHealth()
    {
        if(currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if(currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
            Debug.Log("Health = " + currHealth + " therefore " + this.transform.name + " died.");
        }
    }

    public virtual void Die()
    {
        //was passiert beim Sterben? Spieler, Monster,...
        Debug.Log(transform.name + " died");
    }
}
