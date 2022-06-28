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

    Inventory inventory;


    private void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        attackDamage = playerStats.Attack;
        inventory = Inventory.instance;
    }

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            //Close range attack is initiated in Playercontroller
            if (inventory.fireball)
            {
                if (InputManager.GetActionDown(InputManager.fireball))
                {
                    Fireball();
                }
            }
            if(Inventory.instance.heal == true && Inventory.instance.heals>0)
            {
                if (InputManager.GetActionDown(InputManager.heal))
                {
                    Heal();
                }
            }
        }
    }

    public void Attack()
    {
        Debug.LogWarning("ICh kann angreifen: " + (Time.time < nextAttackTime));
        if (Time.time < nextAttackTime)
            return;

        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log(hitEnemies.Length);

        int damage = Inventory.instance.GetAttackModifier()+playerStats.Attack;
        knockbackForce = damage * 20;

        foreach(Collider2D enemy in hitEnemies)
        {
            //Debug.Log(enemy.name + " getroffen!");
            //Damage Calculation
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            //Debug.Log(enemy.GetComponent<Enemy>().TakeDamage(damage));
            //Knockback Calculation
            Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        nextAttackTime = Time.time + 1f / attackRate;
    }

    void Fireball()
    {
        if (Time.time < nextAttackTime)
            return;

        Instantiate(fireballProjectile, rangeAttackPoint.position, Quaternion.identity);
        nextAttackTime = Time.time + 1f / fireBallCooldown;
    }
    void Heal()
    {
        if (Time.time < nextAttackTime)
            return;

        //animator
        playerStats.Heal(Inventory.instance.GetMagicModifier()+playerStats.Magic);
        Inventory.instance.heals--;
        
        for(int i=0; i < 10; i++)
        {
            if (Inventory.instance.items[i].name == "Heal")
            {
                Debug.Log("Found heal");
            }
        }

        //der findet heal nicht
        //int index = Inventory.instance.items.IndexOf(Heal);

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
