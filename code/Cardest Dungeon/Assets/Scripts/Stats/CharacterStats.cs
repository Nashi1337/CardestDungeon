using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int CurrHealth { get { return currHealth; } private set { currHealth = value; } }
    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
    public int Attack { get { return attack; } private set { attack = value; } }
    public int Defense { get { return defense; } private set { defense = value; } }
    public bool IsDead { get { return isDead; } private set { isDead = value; } }

    
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defense;
    private int currHealth;
    private bool isDead;

	public HealthBar healthBar;

    private void Awake()
    {
        CurrHealth = MaxHealth;
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
        Damage -= Defense;
        Damage = Mathf.Clamp(damage, 0, int.MaxValue);

        CurrHealth -= Damage;

        healthBar.SetHealth(currHealth);

        Debug.Log(transform.name + " takes " + damage + " damage. servus");

        CheckHealth();

    }

    public virtual void CheckHealth()
    {
        if(CurrHealth >= MaxHealth)
        {
            CurrHealth = MaxHealth;
        }
        if(CurrHealth <= 0)
        {
            CurrHealth = 0;
            IsDead = true;
            Debug.Log("Health = " + CurrHealth + " therefore " + transform.name + " died.");
        }
    }

    public virtual void Die()
    {
        //was passiert beim Sterben? Spieler, Monster,...
        Debug.Log(transform.name + " died");
    }
}
