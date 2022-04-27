using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashScript : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DungeonEnemy enemy = collision.gameObject.GetComponent<DungeonEnemy>();
            enemy.TakeDamage(PlayerController.Current.attack + Inventory.instance.GetAttackModifier());
            Vector2 knockbackDirection = (collision.transform.position - gameObject.transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * 20, ForceMode2D.Impulse);
        }
    }
}
