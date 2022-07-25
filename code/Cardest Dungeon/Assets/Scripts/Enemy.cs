using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class represents an enemy behaviour within the dungeon.
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyStats EnemyStats { get { return enemyStats; } }

    private bool attackAvailable = true;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    protected float detectRange;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float distance;
    [SerializeField]
    protected bool boss;
    private float accelerationfactor = 1;
    [SerializeField]
    private AudioSource grindSound;
    [SerializeField]
    private AudioSource dieSound;

    [SerializeField]
    private Interactable dropAttack;
    [SerializeField]
    private Interactable dropDefense;
    [SerializeField]
    private Interactable dropMagic;
    [SerializeField]
    private Interactable dropBoss;
    [SerializeField]
    private Sprite defeatedBoss;

    [SerializeField]
    private int zahltrigger;

    private Vector3 directionToPlayer;

    protected EnemyStats enemyStats;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriterenderer;

    private DialogueManager dm;

    public int zahl = 0;

    public GameObject fireballProjectile;
    public float fireBallCooldown = 3f;
    private float nextAttackTime = 0f;
 

    //public bool Loading = true;

    private bool isdead;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        SetScale();
    }

    public void SetScale()
    {
        float scaleModifier = 0.4f;
        scaleModifier += enemyStats.Defense / 15f;
        if (enemyStats.Magic != 0)
        {
            scaleModifier += enemyStats.Attack / 45f;
            scaleModifier += enemyStats.Magic / 15f;
        }
        else
        {
            scaleModifier += enemyStats.Magic / 30f;
            scaleModifier += enemyStats.Attack / 15f;
        }
        Transform tempParent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one * scaleModifier;
        transform.SetParent(tempParent, true);
    }

    public void Initialize()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        enemyStats = GetComponent<EnemyStats>();
        dm = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boss == true)
        {
            if (Time.time >= nextAttackTime)
            {
                if (Vector2.Distance(PlayerController.Current.transform.position, transform.position) <= detectRange)
                {
                    ShootFireBallTowardsPlayer();
                    nextAttackTime = Time.time + 1f / fireBallCooldown;
                }
            }
        }

        //movement for ranged enemies
        if (this.tag == "RangeEnemy")
        {
            //Enemies that attack with ranged attacks have this tag
            //When the set distance for the enemy is greater than their distance to the player, they will move away from the player
            if(Vector2.Distance(PlayerController.Current.transform.position, transform.position) < distance)
            {
                accelerationfactor = -1;
                if(Time.time >= nextAttackTime)
                {
                    ShootFireBallTowardsPlayer();
                    nextAttackTime = Time.time + 1f / fireBallCooldown;
                }
            }
            else
            {
                accelerationfactor = 1;
            }
        }



        if (Vector2.Distance(PlayerController.Current.transform.position, transform.position) <= detectRange)
        {
            //if (!grindSound.isPlaying){grindSound.Play();}
            MoveEnemy();
            if (animator != null)
            {
                animator.SetBool("chasingPlayer", true);
            }
            float enemyPlayerDeltaX = PlayerController.Current.transform.position.x - transform.position.x;
            spriterenderer.flipX = enemyPlayerDeltaX < 0;
        }
        else
        {
            //if (grindSound.isPlaying){grindSound.Pause();}
            rb.velocity = Vector2.zero;
            if (animator != null)
            {
                animator.SetBool("chasingPlayer", false);
            }
        }

        if (zahl != 0)
        {
            //Debug.Log(this.name + " says: " + dm.name + " " + dm.read[zahl]);
            if (dm.read[zahl] == zahl && isdead==false)
            {
                zahl = 0;
                Die();
                isdead = true;
            }
        }
    }

    private void ShootFireBallTowardsPlayer()
    {
        Vector3 enemyToPlayer = PlayerController.Current.transform.position - transform.position;
        float angle = Mathf.Atan2(enemyToPlayer.y, enemyToPlayer.x) * Mathf.Rad2Deg;

        GameObject fireball = Instantiate(fireballProjectile, transform.position, Quaternion.Euler(0, 0, angle));
        EvilProjectile evil = fireball.GetComponent<EvilProjectile>();
        evil.damage = enemyStats.Magic;
        evil.targetDir = (PlayerController.Current.transform.position - transform.position).normalized;
        evil.enemy = this;
    }

    /// <summary>
    /// Contains the movement pattern of the enemy which is to directly move into the players direction.
    /// </summary>
    private void MoveEnemy()
    {
        directionToPlayer = (PlayerController.Current.transform.position - this.transform.position).normalized;
        rb.AddForce(directionToPlayer * acceleration * accelerationfactor);

        //Attack should not happen in MoveEnemy. This needs to be moved somewhere else
        //CharacterStats targetStats = player.GetComponent<CharacterStats>();
        //if (targetStats != null)
        //{
        //    combat.Attack(targetStats);
        //}
    }

    private void LateUpdate()
    {
        Vector3 healthbarScale = enemyStats.GetHealthBar().transform.localScale;
        enemyStats.GetHealthBar().transform.localScale = healthbarScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attackAvailable && tag != "Obstacle" && collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(enemyStats.Attack, enemyStats);
            attackAvailable = false;
            StartCoroutine(AttackCooldown());
        }
    }

    //public int GetIndex()
    //   {
    //       return enemyIndex;
    //   }

    //public int GetHealth()
    //{
    //    return health;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue"></param>
    /// <returns>Actual damage taken.</returns>
    public int TakeDamage(int attackValue, CharacterStats attacker)
    {
        int actualDamage = enemyStats.TakeDamage(attackValue, attacker);
        if(actualDamage < 0)
        {
            actualDamage = 1;
        }
        //animator.SetBool("Hurt", true);

        if (animator != null)
        {
            animator.SetTrigger("isHit");
        }

        if(enemyStats.IsDead)
        {
            Die();
        }
        return actualDamage;
    }

    public int Attack(CharacterStats victim)
    {
        if (victim != null)
        {
            return victim.TakeDamage(enemyStats.Attack, enemyStats);
        }
        return 0;
    }

    public void Die()
    {
        if (this.tag == "Obstacle")
        {
            //Debug.Log("Die Die Methode wird so oft aufgerufen");
            StartCoroutine(DeactivateObjectAfterWait());
        }
        else
        {
            if (zahltrigger != 0)
            {
                dm.read[zahltrigger] = zahltrigger;
            }

            detectRange = -1;
            nextAttackTime = float.MaxValue;

            if (animator != null)
            {
                animator.SetBool("chasingPlayer", false);
                animator.SetBool("isDead", true);
            }
            Destroy(GetComponent<Collider2D>());
            if (dieSound != null)
            {
                dieSound.Play();
            }

            Vector3 spawnPosition = transform.position;
            spawnPosition.z -= 1;

            if (boss == true)
            {
                //spawnPosition.y += 7;
                //Instantiate(dropBoss, new Vector3(70,83,-1), Quaternion.identity);
                Instantiate(dropBoss, this.transform.position + new Vector3(0,0,5), Quaternion.identity);
                transform.localPosition = transform.localPosition + new Vector3(0,0,-1);
                spriterenderer.sortingOrder = 0;
            }
            else if (UnityEngine.Random.Range(0, 100) <= 50)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (random <= 33)
                {
                    if (dropAttack != null)
                    {
                        Instantiate(dropAttack, spawnPosition, Quaternion.identity);
                    }
                }
                else if (random <= 66)
                {
                    if (dropAttack != null)
                    {
                        Instantiate(dropDefense, transform.position, Quaternion.identity);
                    }
                }
                else
                {
                    if (dropAttack != null)
                    {
                        Instantiate(dropMagic, transform.position, Quaternion.identity);
                    }
                }
            }
        }

        if(grindSound != null)
        {
            grindSound.Pause();
        }

        rb.isKinematic = true; //Disables enemy physics
        rb.velocity = Vector3.zero;
        if (boss == true)
        {
            PlayerController.Current.bossDefeated = true;
            if (defeatedBoss != null)
            {
                spriterenderer.sprite = defeatedBoss;
            }
            
            EnemyBoss2 boss2 = GetComponent<EnemyBoss2>();
            if(boss2 != null)
            {
                boss2.KillstateMachine();

                //Initiate credits
                GameTime.IsGamePaused = true;
                FadeOutToCredits fff = gameObject.AddComponent<FadeOutToCredits>();
                fff.timeToFadeOut = 5;
            }

            dm.NextDungeon();
        }
        else if (tag != "Obstacle")
        {
            StartCoroutine(DieWithDelay());
        }
    }

    IEnumerator DieWithDelay()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }

    IEnumerator DeactivateObjectAfterWait()
    {
        yield return new WaitForSeconds(1.0f);
        dieSound.Play();
        //dieSound.enabled = false;
        Destroy(GetComponent<Collider2D>());
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
        spriterenderer.sortingOrder = 0;
        transform.position += new Vector3(0, 0, -0.1f);
        enabled = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        attackAvailable = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}