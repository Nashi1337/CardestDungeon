using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTEST : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public Transform rangeAttackPoint;
    public float attackRange = 0.5f;
    public int attackDamage;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float knockbackForce;
    public float fireBallCooldown = 3f;

    public LayerMask enemyLayers;

    PlayerStats playerStats;
    public GameObject fireballProjectile;



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
            if (Inventory.instance.fireball== true)
            {
                if (Input.GetKeyDown(InputManager.fireball))
                {
                    Fireball();
                    nextAttackTime = Time.time + 1f / fireBallCooldown;
                }
            }
            if(Inventory.instance.heal == true && Inventory.instance.heals>0)
            {
                if (Input.GetKeyDown(InputManager.heal))
                {
                    Heal();
                }
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log(hitEnemies.Length);

        int damage = Inventory.instance.GetAttackModifier()+playerStats.Attack;
        knockbackForce = damage * 20;

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " getroffen!");
            //Damage Calculation
            //enemy.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log(enemy.GetComponent<Enemy>().TakeDamage(damage));
            //Knockback Calculation
            Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Fireball()
    {
        //animator

        Instantiate(fireballProjectile, rangeAttackPoint.position, Quaternion.identity);

    }
    void Heal()
    {
        //animator
        playerStats.Heal(Inventory.instance.GetMagicModifier());
        Inventory.instance.heals--;
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
