using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTEST : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float acceleration;

    private Vector3 directionToPlayer;
    private Vector3 localScale;
    private Rigidbody2D rb;
    private PlayerControllerTEST player;

    EnemyStats enemystats;

    CharacterCombat combat;

    EnemyUITEST enemyUI;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<PlayerControllerTEST>();

        localScale = transform.localScale;

        combat = GetComponent<CharacterCombat>();

        enemystats = GetComponent<EnemyStats>();

        enemyUI = GetComponent<EnemyUITEST>();
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
        directionToPlayer = (player.transform.position - this.transform.position).normalized;
        rb.AddForce(directionToPlayer * acceleration);
        CharacterStats targetStats = player.GetComponent<CharacterStats>();
        if(targetStats != null)
        {
            combat.Attack(targetStats);
        }
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }

        enemystats.CheckHealth();
        enemyUI.enemyhealth.text = currentHealth.ToString();
        enemystats.healthBar.SetHealth(currentHealth);
    }

    void Die()
    {
        Debug.Log(this.name + " died");

        animator.SetBool("IsDead", true);

        //GetComponent<Collider2D>().enabled = false;

        this.enabled = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
