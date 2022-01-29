using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an enemy behaviour within the dungeon.
/// </summary>
public class DungeonEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject battleEnemyToLoad;
    [SerializeField]
    private static bool isdead;
    private Rigidbody2D rb;
    private PlayerController player;
    private float moveSpeed;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    bool created = false;
    public bool Loading = true;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Ich bin " + this.name + " und gebe in Start als Player aus: " + player);
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = FindObjectOfType<PlayerController>();
        Debug.Log(this.name + "Nachdem ich den Player suche gebe ich aus: " + player);
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
        Debug.Log("Ich bin " + this.name + " MoveEnemy und gebe als Player aus: " + player);
        directionToPlayer = (player.transform.position - this.transform.position).normalized;
        Debug.Log("Nachdem " + this.name + " die directionToPlayer bestimme gebe ich aus: " + player);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The equivalent of this enemy for the battle.</returns>
    public GameObject GetBattleObject()
    {
        return battleEnemyToLoad;
    }
}