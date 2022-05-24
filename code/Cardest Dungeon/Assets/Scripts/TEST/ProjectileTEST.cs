using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTEST : MonoBehaviour
{
    public float moveSpeed = 15f;
    public int damage;
    private bool left = false;

    PlayerController player;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player.GetComponent<Transform>().localScale.x < 0)
            left = true;
        damage += Inventory.instance.GetMagicModifier();
        StartCoroutine(ProjectileLifespan());
    }

    private void Update()
    {
        if (left)
        {
            transform.position = new Vector3(
            transform.position.x - (moveSpeed * Time.deltaTime),
            transform.position.y,
            transform.position.z
            );
        }
        else
        {

            transform.position = new Vector3(
                transform.position.x + (moveSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
                );
        }
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
