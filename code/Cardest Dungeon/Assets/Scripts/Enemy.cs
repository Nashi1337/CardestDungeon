using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an enemy behaviour within the dungeon.
/// </summary>
public class Enemy : MonoBehaviour
{
    //[SerializeField]
    //private int enemyIndex;
    //[SerializeField]
    //private int health;
    //[SerializeField]
    //private GameObject battleEnemyToLoad;
    private bool attackAvailable = true;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float distance;
    [SerializeField]
    private bool boss;
    private float accelerationfactor = 1;

    private Vector3 directionToPlayer;
    private Rigidbody2D rb;

    public AudioSource[] audioSource;
    public AudioSource grindSound;
    public AudioSource dieSound;

    [SerializeField]
    private Animator animator;
    private EnemyStats enemystats;
    DialogueManager dm;

    private SpriteRenderer spriterenderer;

    public int zahl = 0;

    public GameObject fireballProjectile;
    public float fireBallCooldown = 3f;
    private float nextAttackTime = 0f;
 

    //public bool Loading = true;

    private bool isdead;


    // Start is called before the first frame update
    void Start()
    {
        //if (EnemyManager.Instance.HasMyTimeCome(enemyIndex))
        //{
        //    Destroy(gameObject);
        //}

        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponents<AudioSource>();
        grindSound = audioSource[0];
        dieSound = audioSource[1];
        enemystats = GetComponent<EnemyStats>();
        spriterenderer = GetComponent<SpriteRenderer>();
        dm = FindObjectOfType<DialogueManager>();


        //Scale calculation. Falls Diesen Block löschen, falls es nicht gut aussieht.
        float scaleModifier = 0.4f;
        scaleModifier += enemystats.Defense / 15f;
        if(enemystats.Magic != 0)
        {
            scaleModifier += enemystats.Attack / 45f;
            scaleModifier += enemystats.Magic / 15f;
        }
        else
        {
            scaleModifier += enemystats.Magic / 30f;
            scaleModifier += enemystats.Attack / 15f;
        }
        transform.localScale = Vector3.one * scaleModifier;
        //Block Ende
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
                    Shoot();
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
                    Shoot();
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
            if (!grindSound.isPlaying)
            {
                grindSound.Play();
            }
            MoveEnemy();
            animator.SetBool("chasingPlayer", true);
            float enemyPlayerDeltaX = PlayerController.Current.transform.position.x - transform.position.x;
            spriterenderer.flipX = enemyPlayerDeltaX < 0;
        }
        else
        {
            if (grindSound.isPlaying)
            {
                grindSound.Pause();
            }
            rb.velocity = Vector2.zero;
            animator.SetBool("chasingPlayer", false);
        }

        if (zahl != 0)
        {
            //Debug.Log(this.name + " says: " + dm.name + " " + dm.read[zahl]);
            if (dm.read[zahl] == zahl && isdead==false)
            {
                Die();
                isdead = true;
            }
        }
    }

    void Shoot()
    {
        GameObject fireball = Instantiate(fireballProjectile, transform.position, Quaternion.identity);
        fireball.GetComponent<EvilProjectile>().damage = enemystats.Magic;
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
        Vector3 scale = transform.localScale;
        Vector3 healthbarScale = enemystats.GetHealthBar().transform.localScale;
        if (rb.velocity.x < 0)
        {
            //scale.x = -scale.x;
            //healthbarScale.x = -healthbarScale.x;

        }
        transform.localScale = scale;
        enemystats.GetHealthBar().transform.localScale = healthbarScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attackAvailable && collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(enemystats.Attack);
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
    public int TakeDamage(int attackValue)
    {
        int actualDamage = enemystats.TakeDamage(attackValue);
        if(actualDamage > 0)
        {
            //animator.SetBool("Hurt", true);

            animator.SetTrigger("isHit");
        }
        if(enemystats.IsDead)
        {
            Die();
        }
        return actualDamage;
    }

    public int Attack(CharacterStats victim)
    {
        if (victim != null)
        {
            return victim.TakeDamage(enemystats.Attack);
        }
        return 0;
    }

    public void Die()
    {
        if (this.tag == "Obstacle")
        {
            StartCoroutine(DeactivateObjectAfterWait());
        }
        else
        {
            detectRange = -1;
            nextAttackTime = float.MaxValue;

            animator.SetBool("chasingPlayer", false);
            animator.SetBool("isDead", true);
            Destroy(GetComponent<Collider2D>());
            dieSound.Play();
            StartCoroutine(DieWithDelay());
        }

        grindSound?.Pause();

        rb.isKinematic = true; //Disables enemy physics
        rb.velocity = Vector3.zero;

        if (boss == true)
        {
            dm.Victory();
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
        animator.SetBool("isDead", true);
        spriterenderer.sortingOrder = 0;
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

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns>The equivalent of this enemy for the battle.</returns>
    //public GameObject GetBattleObject()
    //{
    //    return battleEnemyToLoad;
    //}
}