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
    private Vector3 directionToPlayer;
    private Vector3 localScale; //May become redundant soon
    private Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField]
    private Animator animator;
    private EnemyStats enemystats;
    //public bool Loading = true;

    //[SerializeField]
    //private static bool isdead;


    // Start is called before the first frame update
    void Start()
    {
        //if (EnemyManager.Instance.HasMyTimeCome(enemyIndex))
        //{
        //    Destroy(gameObject);
        //}

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        enemystats = GetComponent<EnemyStats>();
        localScale = transform.localScale;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(PlayerController.Current.transform.position, transform.position) <= detectRange)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            MoveEnemy();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                rb.velocity = Vector2.zero;
                audioSource.Pause();
            }
        }
    }

    /// <summary>
    /// Contains the movement pattern of the enemy which is to directly move into the players direction.
    /// </summary>
    private void MoveEnemy()
    {
        directionToPlayer = (PlayerController.Current.transform.position - this.transform.position).normalized;
        rb.AddForce(directionToPlayer * acceleration);

        //Attack should not happen in MoveEnemy. This needs to be moved somewhere else
        //CharacterStats targetStats = player.GetComponent<CharacterStats>();
        //if (targetStats != null)
        //{
        //    combat.Attack(targetStats);
        //}
    }

    private void LateUpdate()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
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
            animator.SetBool("Hurt", true);
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
        animator.SetBool("IsDead", true);

        enabled = false;
        Destroy(GetComponent<Collider2D>());
        audioSource.Pause();
        transform.position += Vector3.forward;
        rb.isKinematic = true; //Disables enemy physics
        rb.velocity = Vector3.zero;
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