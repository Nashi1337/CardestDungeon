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
    [SerializeField]
    private Transform rangeAttackPoint;
    [SerializeField]
    private float interactionRadius;
    private PlayerStats playerStats;

    //private bool attackAvailable = true;
    private Rigidbody2D rig = default;
    private Rigidbody2D rig2;
    private SpriteRenderer spriterRenderer;
    private GameObject mapeditor = default;
    private GameObject inventoryManager = default;
    private GameObject inventoryUI = default;
    private PlayerCombatTEST playerCombatTEST = null;
    //private Transform[] allchildrenofmap = default;
    //private Transform[] allchildrenofinventory = default;
    //private Item[] inventoryItems = default;

    public static bool canMove = true;
    public static Vector2 currentPosition = new Vector2(-10, -140);
    public float walkDirectionInDegree;
    public float lookDirection;
    public Vector3 lookDirectionAsVector;

    private static PlayerController current = null;
    private static PlayerController playerInstance;

    int quit = 0;

    DialogueManager dm;

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
        playerCombatTEST = GetComponent<PlayerCombatTEST>();

        //inventoryItems = new Item[inventorySize];
        currentPosition = transform.position;


        //Displays first Tutorial message right on game start. Check Message @DialogueManager Script
        //dm = FindObjectOfType<DialogueManager>();
        //dm.Tutorial1();

        //---------------------------------------------------------------------
        //An den Schluss verschoben, weil die immer so viele Probleme machen :(
        //---------------------------------------------------------------------
        AssignMapManager();
        //allchildrenofmap = mapeditor.GetComponentsInChildren<Transform>();
        mapeditor.SetActive(false);

        AssignInventoryManager();
        //allchildrenofinventory = inventoryManager.GetComponentsInChildren<RectTransform>();
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove == true)
        {
            Vector2 walkDirectionAsVector = InputManager.CalculateInputDirection();

            walkDirectionInDegree = Mathf.Atan2(walkDirectionAsVector.y, walkDirectionAsVector.x) * Mathf.Rad2Deg;
            if (walkDirectionAsVector.magnitude > 0)
            {
                lookDirection = walkDirectionInDegree;
                //Debug.Log("Meine Guckrichtung ist: " + lookDirection);
                attackPoint.transform.position = this.transform.position + new Vector3(walkDirectionAsVector.x, walkDirectionAsVector.y, 0);
                //Debug.Log(walkDirectionAsVector);
                rangeAttackPoint.transform.position = this.transform.position + new Vector3(walkDirectionAsVector.x, walkDirectionAsVector.y, 0);
                lookDirectionAsVector = walkDirectionAsVector;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rig.velocity = walkDirectionAsVector * runningSpeed;
            }
            else
            {
                rig.velocity = walkDirectionAsVector * speed;
            }
            animator.SetFloat("walkDirection", walkDirectionInDegree);
            animator.SetFloat("lookDirection", lookDirection);
            //Debug.Log("Ich laufe in Richtung: " + walkDirectionInDegree);
            //a_speed is the parameter that determines wether the walking animation should be played
            animator.SetFloat("a_Speed", rig.velocity.magnitude);
        }
    }

    private void Update()
    {
        if (InputManager.GetActionDown(InputManager.map))
        {
            if (mapeditor == null)
            {
                AssignMapManager();
            }
            ShowHideMap();
        }
        if (InputManager.GetActionDown(InputManager.inventory))
        {
            if (inventoryManager == null)
            {
                AssignInventoryManager();
            }
            ShowHideInventory();
        }

        if (InputManager.GetActionDown(InputManager.action))
        {
            InteractWithInteractables();
        }
        if(InputManager.GetActionDown(InputManager.attack))
        {
            playerCombatTEST.Attack();
        }
        if (InputManager.GetActionDown(InputManager.actionAndAttack))
        {
            bool interactedSuccessfully = InteractWithInteractables();
            if (!interactedSuccessfully)
            {
                playerCombatTEST.Attack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            quit++;
            if (quit >= 15)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private bool InteractWithInteractables()
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();

        Physics2D.OverlapCircle(gameObject.transform.position, interactionRadius, contactFilter.NoFilter(), results);
        bool interactedSuccessfully = false;
        foreach (Collider2D collider in results)
        {
            if (collider.tag.Equals("Interactable"))
            {
                ItemPickup itemPickup = collider.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    bool success = itemPickup.Interact();
                    if (!interactedSuccessfully)
                        interactedSuccessfully = success;
                    break;
                }
            }
        }
        foreach (Collider2D collider in results)
        {
            if (collider.tag.Equals("Interactable"))
            {
                DialogueTrigger dialogueTrigger = collider.GetComponent<DialogueTrigger>();
                if (dialogueTrigger != null)
                {
                    dialogueTrigger.TriggerDialogue();
                    break;
                }
            }
        }
        foreach (Collider2D collider in results)
        {
            if (collider.tag.Equals("Interactable"))
            {
                InteractableMapPiece interactable = collider.GetComponent<InteractableMapPiece>();
                if (interactable != null)
                {
                    bool success = interactable.Interact();
                    if (!interactedSuccessfully)
                        interactedSuccessfully = success;
                    break;
                }
            }
        }

        return interactedSuccessfully;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue"></param>
    /// <returns>Actual damage Taken</returns>
    public int TakeDamage(int attackValue)
    {
        return playerStats.TakeDamage(attackValue);
        
    }

    public static void Die()
    {
        UIManager.instance.isGameOver = true;
    }
    /// <summary>
    /// Switches the state of the visual map between visible and invisible
    /// </summary>
    private void ShowHideMap()
    {
        mapeditor.SetActive(!mapeditor.activeSelf);
        GameTime.UpdateIsGamePaused();
    }

    private void ShowHideInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        GameTime.UpdateIsGamePaused();
    }

    private void AssignMapManager()
    {
        mapeditor = MapManager.Current.gameObject;
    }

    private void AssignInventoryManager()
    {

        //Braucht man den noch?
        inventoryManager = InventoryManager.Current.gameObject;
        //Debug.Log(inventoryManager);
        //inventoryManager = FindObjectOfType<InventoryManager>().gameObject;

        inventoryUI = FindObjectOfType<InventoryUI>().gameObject;

        //Debug.Log(inventoryManager);
    }
    
    private IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        //attackAvailable = true;
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
