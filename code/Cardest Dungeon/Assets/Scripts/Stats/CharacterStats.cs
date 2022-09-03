using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Muss das von MonoBehaviour erben? vllt Stattdessen als Variable im Enemy-/Playerskript
public class CharacterStats : MonoBehaviour
{
    public int CurrHealth { get { return currHealth; } protected set { currHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int Magic { get { return magic; } set { magic = value; } }
    public bool IsDead { get { return isDead; } protected set { isDead = value; } }

    
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defense;
    [SerializeField]
    protected int magic;
    private int currHealth;
    private bool isDead;

    [SerializeField]
	private HealthBar healthBar;

    protected float knockbackModifier = 5;

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

    /// <summary>
    /// base TakeDamage function for all characters, player and enemy
    /// </summary>
    /// <param name="attackValue">attack value of the attacker</param>
    /// <returns>Actual Damge taken.</returns>
    protected int TakeDamage(int attackValue, int defenseValue, CharacterStats attacker)
    {
        //attack value will be reduced by the defenders defense value
        attackValue -= defenseValue;
        //attack value can't be < 0 of course, otherwise it would heal the defender
        attackValue = Mathf.Max(attackValue, 0);

        //health of the defender will be reduced by the new attack value
        CurrHealth -= attackValue;

        UpdateStats();
        CheckHealth();

        return attackValue;
    }

    public virtual int TakeDamage(int attackValue, CharacterStats attacker)
    {
        return TakeDamage(attackValue, Defense, attacker);
    }

    /// <summary>
    /// Heals used by the player refill the health by the magicvalue * 2
    /// </summary>
    /// <param name="magicValue"></param>
    public void Heal(int magicValue)
    {
        CurrHealth += magicValue*2;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
        UpdateStats();
        CheckHealth();
    }

    public virtual void UpdateStats()
    {
        if (this.gameObject.tag == "Player")
        {
            magic = 5 + Inventory.instance.GetMagicModifier();
            maxHealth = 20 + Inventory.instance.GetHPModifier();
            healthBar.SetMaxHealth(maxHealth);
        }
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
            CharacterDie();
        }
    }

    public virtual void CharacterDie()
    {
        if (gameObject.tag.Equals("Player"))
        {
            PlayerStats.Die();
        }
    }

    public HealthBar GetHealthBar()
    {
        return healthBar;
    }

    public void UpdateMaxHealth()
    {
        healthBar.SetMaxHealth(maxHealth);
        UpdateStats();
    }
}
