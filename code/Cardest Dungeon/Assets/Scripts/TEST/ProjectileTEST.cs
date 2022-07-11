using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTEST : MonoBehaviour
{
    public float moveSpeed = 1f;
    public int damage;

    PlayerController player;

    Vector3 flightDirection;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        damage += Inventory.instance.GetMagicModifier();
        StartCoroutine(ProjectileLifespan());
        flightDirection = player.lookDirectionAsVector;
    }

    private void Update()
    {
            transform.position = new Vector3(
                transform.position.x + (moveSpeed * Time.deltaTime * flightDirection.x),
                transform.position.y + (moveSpeed * Time.deltaTime * flightDirection.y),
                transform.position.z
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision.gameObject.name);
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        //Fireball is not supposed to collide with Player
        if (collision.gameObject.tag.Equals("Projectile") || collision.gameObject.tag.Equals("Player"))
        {
            //mach goar nix
            Debug.Log(collision.gameObject.name + "a");
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Debug.Log(collision.gameObject.name + "b");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Equals("DungeonWall"))
        {
            Debug.Log(collision.gameObject.name + "c");
            Destroy(gameObject);
        }        
    }

    IEnumerator ProjectileLifespan()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
