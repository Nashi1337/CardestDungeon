using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current
    {
        get
        {
            if (current == null)
            {
                current = FindObjectOfType<PlayerController>();
            }
            return current;
        }
    }

    public Item[] Inventory { get; }
    public Animator animator; //animator Variable um für den Player Animationen zu steuern
    public int InventorySize
    {
        get { return inventorySize; }
        set
        {
            if (value > 1)
            {
                inventorySize = value;
            }
        }
    }

    public StateMachine gamestatemachine = new StateMachine();

    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private int inventorySize;
    [SerializeField]
    private GameObject battlescreen;

    private Rigidbody2D rig = default;
    private SpriteRenderer spriterRenderer = default;
    private GameObject mapeditor = default;
    private Transform[] allchildren = default;
    private Item[] inventory = default;

    private static PlayerController current = null;

    //Audio kram
    AudioSource audioData;

    //DEBUG Variablen
    public AudioClip test;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        mapeditor = MapManager.Current.gameObject;
        allchildren = mapeditor.GetComponentsInChildren<Transform>();
        spriterRenderer = GetComponent<SpriteRenderer>();

        //MapEditor and it's children are set false at the start of the game so that the M button action works
        mapeditor.SetActive(false);
        battlescreen.SetActive(false);

        inventory = new Item[inventorySize];

        //audio kram
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rig.velocity = InputManager.CalculateMovement() * runningSpeed;
        }
        else
        {
            rig.velocity = InputManager.CalculateMovement() * speed;
        }
        //Der Parameter a_Speed ist wichtig für die Animation, bei einem Wert > 0.01 wird die walking Animation getriggert
        animator.SetFloat("a_Speed", rig.velocity.magnitude);

        if (rig.velocity.x < 0)
        {
            spriterRenderer.flipX = true;
        }
        else if (rig.velocity.x > 0)
        {
            spriterRenderer.flipX = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.map))
        {
            ShowHideMap();
        }

        gamestatemachine.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gamestatemachine.ChangeState(new BattleState(battlescreen));
        audioData.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MapManager.Current.UpdatePlayerPiece(MapManager.Current.SearchMapPiece(collision.gameObject));
    }

    private void ShowHideMap()
    {
        mapeditor.SetActive(!mapeditor.activeSelf);

        //foreach (Transform child in allchildren)
        //{
        //    child.gameObject.SetActive(mapeditor.activeSelf);
        //}
    }

    /// <summary>
    /// Adds an item to the player's inventory
    /// </summary>
    /// <param name="item">the item that should be added</param>
    /// <returns>True, if item was successfully added. False, if inventory was already full</returns>
    private bool AddToInventory(Item item)
    {
        int index = 0;
        while (index < inventory.Length && inventory[index] != null)
        {
            index++;
        }

        if (index == inventory.Length)
        {
            return false;
        }

        inventory[index] = item;
        return true;
    }

    /// <summary>
    /// Removes the given Item from the inventory and moves all later items one index to the left.
    /// </summary>
    /// <param name="item">The item to be removed</param>
    /// <returns>Returns the removed item or null if no item could be removed</returns>
    private Item RemoveFromInventory(Item item)
    {
        int index = 0;
        while (index < inventory.Length && inventory[index] != item)
        {
            index++;
        }
        Item removed = null;

        //If the item was found in the inventory
        if (index != inventory.Length)
        {
            removed = inventory[index];
            index++;
            while (index < inventory.Length)
            {
                inventory[index - 1] = inventory[index];
            }
        }

        return removed;
    }
}
