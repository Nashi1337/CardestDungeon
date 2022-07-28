using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTEST : MonoBehaviour
{
    public float moveSpeed;
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
        //moves the fireball in the player's looking direction
            transform.position = new Vector3(
                transform.position.x + (moveSpeed * Time.deltaTime * flightDirection.x),
                transform.position.y + (moveSpeed * Time.deltaTime * flightDirection.y),
                transform.position.z
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        //Fireball is not supposed to collide with Player
        if (collision.gameObject.tag.Equals("Projectile") || collision.gameObject.tag.Equals("Player"))
        {
            
        }
        //if collided with an enemy, deal damage
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            if(enemy != null)
            {
                enemy.TakeDamage(damage, player.PlayerStats);
            }
            Destroy(gameObject);
        }
        //if collided with a wall, disappear
        if (collision.gameObject.tag.Equals("DungeonWall"))
        {
            Destroy(gameObject);
        }        
    }

    /// <summary>
    /// destroys the fireball after 3 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator ProjectileLifespan()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
