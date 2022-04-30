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

    [SerializeField]
    private int inventorySize; 
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float attackRadius = 0.5f;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private Animator animator; //animator Variable um für den Player Animationen zu steuern
    [SerializeField]
    private Transform attackPoint;
    private PlayerStats playerStats;

    private bool attackAvailable = true;
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

    void Start()
    {
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
        spriterRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        //AssignMapManager();
        //allchildrenofmap = mapeditor.GetComponentsInChildren<Transform>();
        //mapeditor.SetActive(false);
        
        //AssignInventoryManager();
        //allchildrenofinventory = inventoryManager.GetComponentsInChildren<RectTransform>();
        //inventoryUI.SetActive(false);

        //inventoryItems = new Item[inventorySize];
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
                Debug.LogError("Renderer missing");

            if (rig.velocity.x < 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * -1;
                transform.localScale = scale;
            }
            else if (rig.velocity.x > 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

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
            if (inventoryManager == null)
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
                    collider.GetComponent<ItemPickup>().Interact();
                    break;
                }
            }
        }

/*        if (Input.GetKeyDown(InputManager.attack))
        {
            if (attackAvailable)
            {
                Attack();
                attackAvailable = false;

                StartCoroutine(StartAttackCooldown());
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MapManager.Current != null)
        {
            MapPiece collidedWith = MapManager.Current.SearchMapPiece(collision.gameObject);
            if (collidedWith != null)
            {
                MapManager.Current.UpdatePlayerPiece(collidedWith);
            }
        }
    }

/*    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, LayerMask.GetMask("Enemies"));
        int attackModifier = Inventory.instance.GetAttackModifier();

        foreach (Collider2D enemy in hitEnemies)
        {
            int actualDamage = enemy.GetComponent<Enemy>().TakeDamage(playerStats.Attack + attackModifier);

            //Knockback Calculation
            Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
            float knockbackForce = actualDamage * 20;
            enemy.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue"></param>
    /// <returns>Actual damage Taken</returns>
    public int TakeDamage(int attackValue)
    {
        return playerStats.TakeDamage(attackValue);
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
        mapeditor = MapManager.Current?.gameObject;
    }

    private void AssignInventoryManager()
    {
        //Inventory temp = Inventory.Instance;
        //Debug.Log(Inventory.Instance);
        inventoryManager = InventoryManager.Current.gameObject;
        inventoryUI = FindObjectOfType<InventoryUI>().gameObject;
    }

    ///// <summary>
    ///// Adds an item to the player's inventory
    ///// </summary>
    ///// <param name="item"> the item that should be added</param>
    ///// <returns>True, if item was successfully added. False, if item could not be edited because inventory was already full</returns>
    //public bool AddToInventory(Item item)
    //{
    //    int index = 0;
    //    while (index < inventoryItems.Length && inventoryItems[index] != null)
    //    {
    //        index++;
    //    }

    //    if (index == inventoryItems.Length)
    //    {
    //        return false;
    //    }

    //    inventoryItems[index] = item;
    //    return true;
    //}

    ///// <summary>
    ///// Removes the given Item from the inventory and moves all later items one index to the left.
    ///// </summary>
    ///// <param name="item"> The item to be removed</param>
    ///// <returns>Returns the removed item or null if no item could be removed</returns>
    //public Item RemoveFromInventory(Item item)
    //{
    //    int index = 0;
    //    while (index < inventoryItems.Length && inventoryItems[index] != item)
    //    {
    //        index++;
    //    }
    //    Item removed = null;

    //    //If the item was found in the inventory
    //    if (index != inventoryItems.Length)
    //    {
    //        removed = inventoryItems[index];
    //        index++;
    //        while (index < inventoryItems.Length)
    //        {
    //            inventoryItems[index - 1] = inventoryItems[index];
    //        }
    //    }

    //    return removed;
    //}
    
    private IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        attackAvailable = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
