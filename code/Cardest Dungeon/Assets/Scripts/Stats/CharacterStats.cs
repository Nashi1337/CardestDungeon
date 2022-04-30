using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Muss das von MonoBehaviour erben? vllt Stattdessen als Variable im Enemy-/Playerskript
public class CharacterStats : MonoBehaviour
{
    public int CurrHealth { get { return currHealth; } private set { currHealth = value; } }
    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
    public int Attack { get { return attack; } protected set { attack = value; } }
    public int Defense { get { return defense; } protected set { defense = value; } }
    public int Magic { get { return magic; } protected set { magic = value; } }
    public bool IsDead { get { return isDead; } protected set { isDead = value; } }

    
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int magic;
    private int currHealth;
    private bool isDead;

    [SerializeField]
	private HealthBar healthBar;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CurrHealth = MaxHealth;
        healthBar.SetMaxHealth(maxHealth);
        UpdateStats();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue"></param>
    /// <returns>Actual Damge taken.</returns>
    protected int TakeDamage(int attackValue, int defenseValue)
    {
        attackValue -= defenseValue;
        attackValue = Mathf.Max(attackValue, 0);

        CurrHealth -= attackValue;

        UpdateStats();

        Debug.Log(transform.name + " takes " + attackValue + " damage. characterstats");

        CheckHealth();

        return attackValue;
    }

    public virtual int TakeDamage(int attackValue)
    {
        return TakeDamage(attackValue, Defense);
    }

    public void Heal(int magicValue)
    {
        CurrHealth += magicValue*10;
        UpdateStats();
        CheckHealth();
    }

    public virtual void UpdateStats()
    {
        healthBar.SetHealth(currHealth);
    }

    public virtual void CheckHealth()
    {
        if(CurrHealth >= MaxHealth)
        {
            CurrHealth = MaxHealth;
        }
        if(CurrHealth <= 0)
        {
            IsDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        //was passiert beim Sterben? Spieler, Monster,...
        //Debug.Log(transform.name + " died");
    }
}
