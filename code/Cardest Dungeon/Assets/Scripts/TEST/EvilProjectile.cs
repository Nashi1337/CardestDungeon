using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int damage = 6;

    public Vector3 targetDir;

    private void Start()
    {
        StartCoroutine(ProjectileLifespan());
    }

    private void FixedUpdate()
    {
        transform.position += targetDir * Time.fixedDeltaTime * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Bin mit " + collision + " kollidiert.");

        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        //Evil Fireball is not supposed to collide with anything else
        if (collision.gameObject.tag.Equals("Player"))
        {
            //Debug.Log("Hab den Spieler erwischt");
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if(collision.gameObject.tag.Equals("RangeEnemy"))
        {
            //Do nothing since we don't want the projectile to hit the shooter
        }
        else if (collision.gameObject.tag.Equals("DungeonWall"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ProjectileLifespan()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
