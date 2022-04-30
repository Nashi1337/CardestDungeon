using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTEST : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //public int maxHealth = 100;
    //int currentHealth;

    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float acceleration;

    private Vector3 directionToPlayer;
    private Vector3 localScale;
    private Rigidbody2D rb;
    private PlayerController player;

    EnemyStats enemystats;

    CharacterCombat combat;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        combat = GetComponent<CharacterCombat>();
        enemystats = GetComponent<EnemyStats>();
        player = PlayerController.Current;


        //currentHealth = maxHealth;
        localScale = transform.localScale;

    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(player.transform.position, transform.position) <= detectRange)
        {
            MoveEnemy();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void MoveEnemy()
    {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.AddForce(directionToPlayer * acceleration);
        //CharacterStats targetStats = player.GetComponent<CharacterStats>();
        //if(targetStats != null)
        //{
        //    combat.Attack(targetStats);
        //}
    }

    private void LateUpdate()
    {
        if(rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else if(rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>Actual damage taken.</returns>
    public int TakeDamage(int damage)
    {
        return enemystats.TakeDamage(damage);
    }

    void Die()
    {
        Debug.Log(name + " died");

        animator.SetBool("IsDead", true);

        enabled = false;
        Destroy(GetComponent<Collider2D>());
        transform.position += Vector3.forward;
        rb.isKinematic = true; //Disables enemy physics
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
