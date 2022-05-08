using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTEST : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damage;
    private bool left = false;

    PlayerController player;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player.GetComponent<Transform>().localScale.x < 0)
            left = true;
        damage = Inventory.instance.GetMagicModifier();
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

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        //Fireball is not supposed to collide with Player
        if (!collision.gameObject.tag.Equals("Player"))
            {

            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
