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

    public Animator animator; //animator Variable um für den Player Animationen zu steuern

    [SerializeField]
    private int inventorySize;
    
    //Set this on private when the script has been merged whith PlayerAttack_MergeWithPlayer.cs
    [SerializeField]
    public int attack;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private string battleSceneName;
    [SerializeField]
    private CharacterStatus playerStatus;

    private Rigidbody2D rig = default;
    private Rigidbody2D rig2;
    private SpriteRenderer spriterRenderer;
    private GameObject mapeditor = default;
    private GameObject inventoryManager = default;
    private GameObject inventoryUI = default;
    private Transform[] allchildrenofmap = default;
    private Transform[] allchildrenofinventory = default;
    private Item[] inventoryItems = default;

    public static bool canMove = true;
    public static Vector2 currentPosition = new Vector2(-10, -140);

    private static PlayerController current = null;
    private static PlayerController playerInstance;
    private float interactionRadius = 2f;

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
        //DontDestroyOnLoad(gameObject);

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
        AssignMapManager();
        allchildrenofmap = mapeditor.GetComponentsInChildren<Transform>();
        AssignInventoryManager();
        allchildrenofinventory = inventory.GetComponentsInChildren<RectTransform>();

        //Debug.Log(mapeditor);
        //Debug.Log(allchildrenofmap);
        //Debug.Log(inventory);
        //Debug.Log(allchildrenofinventory);

        spriterRenderer = GetComponent<SpriteRenderer>();

        //MapEditor and Inventory and it's children are set false at the start of the game so that the M button action works
        mapeditor.SetActive(false);
        inventoryUI.SetActive(false);

        inventoryItems = new Item[inventorySize];
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

            if (spriterRenderer == null)
                Debug.Log("Renderer missing");

            if (rig.velocity.x < 0)
            {
                spriterRenderer.flipX = true;
            }
            else if (rig.velocity.x > 0)
            {
                spriterRenderer.flipX = false;
            }
        }

/*        if (Input.GetKeyDown(InputManager.action))
        {
            TryGetComponent<ItemObject>(out ItemObject item);
            item.OnHandlePickupItem();
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.map))
        {
            if (mapeditor == null)
            {
                AssignMapManager();
            }
            ShowHideMap();
        }
        if (Input.GetKeyDown(InputManager.inventory))
        {
            if (inventory == null)
            {
                AssignInventoryManager();
            }
            ShowHideInventory();
        }

        if (Input.GetKeyDown(InputManager.action))
        {
            List<Collider2D> results;
            results = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();

            Physics2D.OverlapCircle(gameObject.transform.position, interactionRadius, contactFilter.NoFilter(), results);
            foreach(Collider2D collider in results)
            {
                if (collider.tag.Equals("Interactable"))
                {
                    Debug.Log("Servus Erdnuss");
                    collider.GetComponent<ItemPickup>().Interact();
                    break;
                }
            }
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
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    canMove = false;
        //    currentPosition = new Vector2(transform.position.x, transform.position.y);

        //    //We destroy the gameobject that collided with our player, so that it is gone once we reload the scene
        //    EnemyManager.Instance.KillEnemy(collision.gameObject.GetComponent<DungeonEnemy>().GetIndex());
        //    SceneManager.LoadScene(battleSceneName);
        //}
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
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    private void AssignMapManager()
    {
        mapeditor = MapManager.Current.gameObject;
    }

    private void AssignInventory()
    {
        //Inventory temp = Inventory.Instance;
        //Debug.Log(Inventory.Instance);
        inventoryManager = InventoryManager.Current.gameObject;
        inventoryUI = FindObjectOfType<InventoryUI>().gameObject;
    }
    /// <summary>
    /// Adds an item to the player's inventory
    /// </summary>
    /// <param name="item"> the item that should be added</param>
    /// <returns>True, if item was successfully added. False, if item could not be edited because inventory was already full</returns>
    public bool AddToInventory(Item item)
    {
        int index = 0;
        while (index < inventoryItems.Length && inventoryItems[index] != null)
        {
            index++;
        }

        if (index == inventoryItems.Length)
        {
            return false;
        }

        inventoryItems[index] = item;
        return true;
    }

    /// <summary>
    /// Removes the given Item from the inventory and moves all later items one index to the left.
    /// </summary>
    /// <param name="item"> The item to be removed</param>
    /// <returns>Returns the removed item or null if no item could be removed</returns>
    public Item RemoveFromInventory(Item item)
    {
        int index = 0;
        while (index < inventoryItems.Length && inventoryItems[index] != item)
        {
            index++;
        }
        Item removed = null;

        //If the item was found in the inventory
        if (index != inventoryItems.Length)
        {
            removed = inventoryItems[index];
            index++;
            while (index < inventoryItems.Length)
            {
                inventoryItems[index - 1] = inventoryItems[index];
            }
        }

        return removed;
    }
}
