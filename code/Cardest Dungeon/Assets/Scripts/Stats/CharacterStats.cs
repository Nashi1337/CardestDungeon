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
    //private int mana = 0;

    [SerializeField]
	private HealthBar healthBar;

    //[SerializeField]
    //private ManaBar manaBar;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CurrHealth = MaxHealth;

        //mana = magic;
        healthBar.SetMaxHealth(maxHealth);
        //manaBar.SetMaxMana(mana);
        //StartCoroutine(RefillMana());
        UpdateStats();
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    //TakeDamage(10, 0);
        //    UseMana(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    UseMana(-1);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue"></param>
    /// <returns>Actual Damge taken.</returns>
    protected int TakeDamage(int attackValue, int defenseValue)
    {
        //alte Berechnung
        attackValue -= defenseValue;
        attackValue = Mathf.Max(attackValue, 0);

        //NeueBerechnung
        //attackValue = Mathf.RoundToInt((attackValue - (float)defenseValue) / 3 + attackValue / (float)defenseValue);

        CurrHealth -= attackValue;

        UpdateStats();

        //Debug.Log(transform.name + " takes " + attackValue + " damage. characterstats");

        CheckHealth();

        return attackValue;
    }

    //public void UseMana(int used)
    //{
    //    mana = mana - used;
    //    UpdateStats();
    //}

    public virtual int TakeDamage(int attackValue)
    {
        return TakeDamage(attackValue, Defense);
    }

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
        if(this.gameObject.tag=="Player")
            magic = 5 + Inventory.instance.GetMagicModifier();
        healthBar.SetHealth(currHealth);
        //manaBar.SetMaxMana(magic);
        //manaBar.SetMana(mana);
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

    //public ManaBar GetManaBar()
    //{
    //    return manaBar;
    //}

    //IEnumerator RefillMana()
    //{
    //    yield return new WaitForSeconds(15);
    //    if (mana < magic)
    //    {
    //        mana++;
    //        UpdateStats();
    //    }
    //    StartCoroutine(RefillMana());
    //}

    public void UpdateMaxHealth()
    {
        healthBar.SetMaxHealth(maxHealth);
        UpdateStats();
    }
}
