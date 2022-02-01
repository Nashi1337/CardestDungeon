using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an enemy behaviour within the dungeon.
/// </summary>
public class DungeonEnemy : MonoBehaviour
{
    [SerializeField]
    private int enemyIndex;
    [SerializeField]
    private GameObject battleEnemyToLoad;

    private float moveSpeed;
    private Vector3 directionToPlayer;
    private Vector3 localScale;
    private Rigidbody2D rb;
    private PlayerController player;

    public bool Loading = true;
    
    [SerializeField]
    private static bool isdead;


    // Start is called before the first frame update
    void Start()
    {
        if (EnemyManager.Instance.HasMyTimeCome(enemyIndex))
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        //Debug.Log("Ich bin " + this.name + " und gebe in Start als Player aus: " + player);
        player = FindObjectOfType<PlayerController>();
        //Debug.Log(this.name + "Nachdem ich den Player suche gebe ich aus: " + player);
        moveSpeed = 2f;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy();
    }

    /// <summary>
    /// Contains the movement pattern of the enemy which is to directly move into the players direction.
    /// </summary>
    private void MoveEnemy()
    {
        //Debug.Log("Ich bin " + this.name + " MoveEnemy und gebe als Player aus: " + player);
        directionToPlayer = (player.transform.position - this.transform.position).normalized;
        //Debug.Log("Nachdem " + this.name + " die directionToPlayer bestimme gebe ich aus: " + player);
        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed;
    }

    private void LateUpdate()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Spieler beruehrt");
            EnemyManager.Instance.KillEnemy(enemyIndex);
        }
    }


	public int GetIndex()
    {
        return enemyIndex;
    }
	
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The equivalent of this enemy for the battle.</returns>
    public GameObject GetBattleObject()
    {
        return battleEnemyToLoad;
    }
}