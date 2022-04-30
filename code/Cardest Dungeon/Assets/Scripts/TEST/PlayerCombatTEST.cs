using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTEST : MonoBehaviour
{
    public int attackDamage;
    public float attackRange = 0.5f;
    public Animator animator;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float knockbackForce;
    [SerializeField]
    private Transform attackPoint;

    public LayerMask enemyLayers;

    PlayerStats playerStats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        attackDamage = playerStats.Attack;
    }

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {

            if (Input.GetKeyDown(InputManager.attack))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        int damage = Inventory.instance.GetAttackModifier();
        knockbackForce = damage * 20;

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " getroffen!");
            //Damage Calculation
            enemy.GetComponent<DungeonEnemy>().TakeDamage(damage - enemy.GetComponent<EnemyStats>().Defense);

            //Knockback Calculation
            Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
