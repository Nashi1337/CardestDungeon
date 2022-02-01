using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the script which handles everything regarding the player in the dungeon scene. It is NOT used in the battle scene.
/// </summary>
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

    public Item[] Inventory { get; }
    public Animator animator; //animator Variable um für den Player Animationen zu steuern

    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private int inventorySize;
    [SerializeField]
    private string battleSceneName;
    [SerializeField]
    private CharacterStatus playerStatus;

    private Rigidbody2D rig = default;
    private SpriteRenderer spriterRenderer = default;
    private GameObject mapeditor = default;
    private GameObject inventory = default;
    private Transform[] allchildrenofmap = default;
    private Transform[] allchildrenofinventory = default;
    //private Item[] inventory = default;

    public static bool canMove = true;
    public static Vector2 currentPosition = new Vector2(-10, -140);

    private static PlayerController current = null;
    private static PlayerController playerInstance;

    //Falls Maxens Datenübertragungsweg benutzt wird, kann das hier gelöscht werden
    // private void OnEnable()
    // {
    // //Datentransfer zwischen Szenen sauberer machen und vollständiger
    // if(BattleData.playerPositionBeforeFight != Vector3.zero)
    // {
    // transform.position = BattleData.playerPositionBeforeFight;
    // }
    // }

    void Start()
    {
        //Das Objekt bleibt bestehen auch bei Szenenwechsel
        DontDestroyOnLoad(gameObject);

        if (playerInstance == null)
        {
            playerInstance = this;
            canMove = true;
        }
        else
        {
            Destroy(gameObject);
            canMove = true;
        }

        rig = GetComponent<Rigidbody2D>();
        mapeditor = MapManager.Current.gameObject;
        allchildrenofmap = mapeditor.GetComponentsInChildren<Transform>();
        inventory = InventoryManager.Current.gameObject;
        allchildrenofinventory = inventory.GetComponentsInChildren<RectTransform>();

        Debug.Log(mapeditor);
        Debug.Log(allchildrenofmap);
        Debug.Log(inventory);
        Debug.Log(allchildrenofinventory);

        spriterRenderer = GetComponent<SpriteRenderer>();

        //MapEditor and Inventory and it's children are set false at the start of the game so that the M button action works
        mapeditor.SetActive(false);
        inventory.SetActive(false);

        //inventory = new Item[inventorySize];
        //Debug.Log("1. " + currentPosition);
        //currentPosition = new Vector2(-10, -140);
        //Debug.Log("2. " + currentPosition);
        currentPosition = transform.position;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove == true)
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.map))
        {
            ShowHideMap();
        }
        if (Input.GetKeyDown(InputManager.inventory))
        {
            ShowHideInventory();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MapPiece collidedWith = MapManager.Current.SearchMapPiece(collision.gameObject);
        if (collidedWith != null)
        {
            MapManager.Current.UpdatePlayerPiece(collidedWith);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("KÄMPFT!!!!!!!");

            //Falls Maxens Datenübertragungsweg benutzt wird, kann das hier gelöscht werden
            //BattleData.playerToLoad = playerBattleObjectToLoad;
            //BattleData.enemiesToLoad = new GameObject[1] { collision.gameObject.GetComponent<DungeonEnemy>().GetBattleObject() };
            //BattleData.playerPositionBeforeFight = transform.position;

            canMove = false;

            //Debug.Log("3. " + currentPosition);
            currentPosition = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log("4. " + currentPosition);

            //We destroy the gameobject that collided with our player, so that it is gone once we reload the scene
            Destroy(collision.gameObject);

            SceneManager.LoadScene(battleSceneName);
        }
    }



    /// <summary>
    /// Switches the state of the visual map between visible and invisible
    /// </summary>
    private void ShowHideMap()
    {
        mapeditor.SetActive(!mapeditor.activeSelf);
    }

    private void ShowHideInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }

    /// <summary>
    /// Adds an item to the player's inventory
    /// </summary>
    /// <param name="item">the item that should be added</param>
    /// <returns>True, if item was successfully added. False, if item could not be edited because inventory was already full</returns>
/*    private bool AddToInventory(Item item)
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
    }*/

    /// <summary>
    /// Removes the given Item from the inventory and moves all later items one index to the left.
    /// </summary>
    /// <param name="item">The item to be removed</param>
    /// <returns>Returns the removed item or null if no item could be removed</returns>
/*    private Item RemoveFromInventory(Item item)
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
    }*/
}
