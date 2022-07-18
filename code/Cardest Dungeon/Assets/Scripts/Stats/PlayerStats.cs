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

    public int mana = 0;

    [SerializeField]
    private ManaBar manaBar;
    [SerializeField]
    private GameObject audioPlayerPrefab;

    public GameObject gameOver;

    new private AudioSource audio;

    private Animator animator;

    void Start()
    {
        Initialize();
        mana = magic;
        manaBar.SetMaxMana(mana);
        StartCoroutine(RefillMana());

        audio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //TakeDamage(10, 0);
            UseMana(1);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseMana(-1);
        }
    }

    //Do not Use this except if you are the PlayerController. Else use either the SetStats or the UpdateStats of player controller
    public override void UpdateStats()
    {
        attackText.text = (Attack + Inventory.instance.GetAttackModifier()).ToString();
        defenseText.text = (Defense + Inventory.instance.GetDefenseModifier()).ToString();
        magicText.text = Magic.ToString();
        healthText.text = CurrHealth.ToString();
        manaBar.SetMaxMana(magic);
        manaBar.SetMana(mana);
        base.UpdateStats();
    }

    public void UseMana(int used)
    {
        mana = mana - used;
        UpdateStats();
    }

    public override int TakeDamage(int attackValue)
    {
        if (attackValue > Defense)
        {
            animator.SetTrigger("isHurt");
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            audioPlayer.GetComponent<AudioSource>().clip = audio.clip;
            //audio.Play();
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
        //Debug.Log("Du bist tot. Hier sollte jetzt etwas passieren.");
        PlayerController.canMove = false;
        PlayerController.Die();
    }

    public ManaBar GetManaBar()
    {
        return manaBar;
    }

    IEnumerator RefillMana()
    {
        yield return new WaitForSeconds(15);
        if (mana < magic)
        {
            mana++;
            UpdateStats();
        }
        StartCoroutine(RefillMana());
    }
}
