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

    [SerializeField]
    private float knockbackModifierForEnemies;
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
        if (Time.time < nextAttackTime)
            return;

        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        int damage = Inventory.instance.GetAttackModifier()+playerStats.Attack;
        

        foreach(Collider2D enemy in hitEnemies)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            if (enem != null)
            {
                //Damage Calculation
                knockbackForce = knockbackModifierForEnemies * enemy.GetComponent<Enemy>().TakeDamage(damage);
                //Knockback Calculation
                Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
                enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        nextAttackTime = Time.time + 1f / attackRate;
    }

    void Fireball()
    {
        if (Time.time < nextAttackTime||playerStats.mana<1)
            return;
        Vector3 rotation;
        rotation.z = PlayerController.Current.lookDirection;
        Instantiate(fireballProjectile, rangeAttackPoint.position, Quaternion.Euler(0,0,PlayerController.Current.lookDirectionAsVector.z));
        playerStats.UseMana(1);
        nextAttackTime = Time.time + 1f / fireBallCooldown;
        //Debug.Log("fireball was fired at position " + rangeAttackPoint.position);
    }
    void Heal()
    {
        if (Time.time < nextAttackTime||playerStats.mana<2)
            return;

        //animator
        playerStats.Heal(Inventory.instance.GetMagicModifier()+playerStats.Magic);
        playerStats.UseMana(2);
        
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
