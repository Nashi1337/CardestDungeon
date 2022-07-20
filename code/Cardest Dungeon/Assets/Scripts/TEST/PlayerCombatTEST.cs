using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTEST : MonoBehaviour
{
    public Animator animator;


    public Transform attackPoint;
    public Transform rangeAttackPoint;
    public int attackDamage;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float knockbackForce;
    public float fireBallCooldown = 3f;
    public GameObject fireballProjectile;
    public LayerMask enemyLayers;



    private PlayerStats playerStats;
    private Inventory inventory;

    [SerializeField]
    private float attackFieldWidth;
    [SerializeField]
    private float attackFieldHeight;
    [SerializeField]
    private float knockbackModifierForEnemies;
    [SerializeField]
    private AudioSource hitSound;
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

        hitSound.Play();
        animator.SetTrigger("attack");

        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Vector2 attackRect;
        if(PlayerController.Current.lookDirection >= 45 || PlayerController.Current.lookDirection <= 90
            || PlayerController.Current.lookDirection >= -135 || PlayerController.Current.lookDirection <= -90)
        {
            attackRect = new Vector2(attackFieldHeight, attackFieldWidth);
        }
        else
        {
            attackRect = new Vector2(attackFieldWidth, attackFieldHeight);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRect, 0, enemyLayers);

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
        float rotation = PlayerController.Current.lookDirection;
        Instantiate(fireballProjectile, rangeAttackPoint.position, Quaternion.Euler(0, 0, rotation));
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

        for (int i = 0; i < 10; i++)
        {
            //if (Inventory.instance.items[i].name == "Heal")
            //{
            //    Debug.Log("Found heal");
            //}
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackFieldWidth, attackFieldHeight, 1));
    }
}
