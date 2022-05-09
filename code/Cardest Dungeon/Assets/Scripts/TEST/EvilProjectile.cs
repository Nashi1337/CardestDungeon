using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilProjectile : MonoBehaviour
{
    public float moveSpeed = 3f;
    private int damage = 3;
    //0 = SW, 1 = NW, 2 = NE, 3 = SE
    private float direction = 0;


    PlayerController target;
    Vector3 targetpos;
    Vector3 targetDir;

    private void Start()
    {
        /*        target = FindObjectOfType<PlayerController>();
                targetpos = target.GetComponent<Transform>().localScale;
                if (targetpos.x < transform.position.x && targetpos.y < transform.position.y)
                {
                    direction = 0f;
                    //accelerationFactor = -1; 
                    Debug.Log(direction);
                }
                else if (targetpos.x > transform.position.x && targetpos.y < transform.position.y)
                {
                    direction = 3f;
                    //accelerationFactor = 1;
                    Debug.Log(direction);
                }
                else if (targetpos.x > transform.position.x && targetpos.y > transform.position.y)
                {
                    direction = 2f;
                    //accelerationFactor = 1;
                    Debug.Log(direction);
                }
                else if (targetpos.x < transform.position.x && targetpos.y > transform.position.y)
                {
                    direction = 1f;
                    //accelerationFactor = 1;
                    Debug.Log(direction);
                }*/
        //damage = Inventory.instance.GetMagicModifier();

        targetDir = (PlayerController.Current.transform.position - transform.position).normalized;


    }

    private void FixedUpdate()
    {
        transform.position += targetDir * Time.fixedDeltaTime * moveSpeed;
    }

    private void Update()
    {
/*        if (direction == 0)
        {
            transform.position = new Vector3(
            transform.position.x - (moveSpeed * Time.deltaTime
            //* accelerationFactor
            ),
            transform.position.y - (moveSpeed * Time.deltaTime
            //*accelerationFactor
            ),
            transform.position.z
            );
        }
        if (direction == 3)
            {
            transform.position = new Vector3(
                transform.position.x + (moveSpeed * Time.deltaTime),
                transform.position.y - (moveSpeed * Time.deltaTime),
                transform.position.z
                );
            }
        if (direction == 2)
        {
            transform.position = new Vector3(
                transform.position.x + (moveSpeed * Time.deltaTime),
                transform.position.y + (moveSpeed * Time.deltaTime),
                transform.position.z
                );
        }
        if (direction == 1)
        {
            transform.position = new Vector3(
                transform.position.x - (moveSpeed * Time.deltaTime),
                transform.position.y + (moveSpeed * Time.deltaTime),
                transform.position.z
                );
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bin mit " + collision + " kollidiert.");

        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        //Evil Fireball is not supposed to collide with anything else
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Hab den Spieler erwischt");
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if(!collision.gameObject.tag.Equals("RangeEnemy"))
        {
            Debug.Log("Bin mit " + collision.tag + " kollidiert");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag.Equals("Dungeon"))
        {
            Destroy(gameObject);
        }
    }
}
