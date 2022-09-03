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
    public int highScore = 0;

    [SerializeField]
    private ManaBar manaBar;
    [SerializeField]
    private GameObject audioPlayerPrefab;
    [SerializeField]
    private Text highScoreText;

    public GameObject gameOver;

    new private AudioSource audio;

    private Animator animator;

    void Start()
    {
        //This calls the Initialize function of the parent class, CharacterStats.
        //It sets the current Health to max Health, then sets the health bar to the maximum value.
        Initialize();
        //Only the player has mana. The current as well as the max mana is always equal to the magic value.
        mana = magic;
        manaBar.SetMaxMana(mana);
        StartCoroutine(RefillMana());

        audio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();


    }
    /// <summary>
    /// Is only used for debugging. 
    /// It was used to check if taking damage works and to refill the mana manually.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //TakeDamage(10, 0);
            //UseMana(1);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //UseMana(-1);
        }
    }

    //Do not Use this except if you are the PlayerController. Else use either the SetStats or the UpdateStats of player controller
    /// <summary>
    /// Updates the hidden numeric values of attack, defense and magic,
    /// as well as the mana bar and, through characterstats, the healthbar
    /// </summary>
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

    /// <summary>
    /// Will be called when shooting fireballs or healing. 
    /// Removes either 1 oder 2 from the player's mana.
    /// </summary>
    /// <param name="used"></param>
    public void UseMana(int used)
    {
        mana = mana - used;
        UpdateStats();
    }
    /// <summary>
    /// Used to remove health from the player upon being the victim of an attack.
    /// If currHealth <= 0, the player dies
    /// </summary>
    /// <param name="attackValue">The value of the attack that hit the player</param>
    /// <param name="attacker">Unused. Value is ignored</param>
    /// <returns></returns>
    public override int TakeDamage(int attackValue, CharacterStats attacker)
    {
        if (attackValue > Defense)
        {
            animator.SetTrigger("isHurt");
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            audioPlayer.GetComponent<AudioSource>().clip = audio.clip;
            audio.Play();

            //Knockback Calculation
            float knockbackForce = knockbackModifier * attackValue;
            Vector2 knockbackDirection = (transform.position - attacker.transform.position).normalized;
            PlayerController.Current.AddKnockback(knockbackDirection * knockbackForce);

        }
        if (CurrHealth <= 0)
        {
            Die();
        }
        return base.TakeDamage(attackValue, Defense + Inventory.instance.GetDefenseModifier(), attacker);
    }
    /// <summary>
    /// Calls the CheckHealth function from characterStats.
    /// It checks wether the currenthealth are exceeding the maximum or are less/equal than 0
    /// </summary>
    public override void CheckHealth()
    {
        base.CheckHealth();
    }
    /// <summary>
    /// Player is unable to move and calls the Die function from PlayerController, 
    /// that opens the game over panel.
    /// </summary>
    public static void Die()
    {
        PlayerController.canMove = false;
        PlayerController.Die();
    }
    /// <summary>
    /// Refills one mana every 12 seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator RefillMana()
    {
        yield return new WaitForSeconds(12);
        if (mana < magic)
        {
            mana++;
            UpdateStats();
        }
        StartCoroutine(RefillMana());
    }
    /// <summary>
    /// increases high score value and updates text in pause menu.
    /// </summary>
    /// <param name="value"></param>
    public void IncreaseHighScore(int value)
    {
        highScore = highScore + value;
        if(highScoreText!=null)
            highScoreText.text = highScore.ToString();
    }
}
