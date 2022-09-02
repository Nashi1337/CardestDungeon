using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// <summary>
    /// inventorysize is set in the editor. By default 10 and was never changed during development, but the possibility exists.
    /// </summary>
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

    public PlayerStats PlayerStats { get { return playerStats; } }

    [SerializeField]
    private int inventorySize; 
    [SerializeField]
    private float attackRate;
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

    [SerializeField]
    private GameObject Pause;


    private Rigidbody2D rig = default;
    private Rigidbody2D rig2;
    private SpriteRenderer spriterRenderer;
    private GameObject mapeditor = default;
    private GameObject inventoryManager = default;
    private GameObject inventoryUI = default;
    private PlayerCombatTEST playerCombatTEST = null;


    public static bool canMove = true;
    public static Vector2 currentPosition = new Vector2(-10, -140);
    public float walkDirectionInDegree;
    public float lookDirection;
    public Vector3 lookDirectionAsVector = new Vector3(1, 0, 0);

    public bool bossDefeated = false;


    private static PlayerController current = null;

    int quit = 0;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriterRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerCombatTEST = GetComponent<PlayerCombatTEST>();

        currentPosition = transform.position;

        //MapManager and InventoryManager must be found before they will be deactivated.
        AssignMapManager();
        mapeditor.SetActive(false);

        AssignInventoryManager();
        inventoryUI.SetActive(false);

        //Pause Menu must be deactivated before the first frame as well.
        if (Pause != null)
        {
            Pause.SetActive(false);
        }

        //When the first dungeon is loaded, the current highscore, which is carried over through all scenes,
        //will be set to the same value but negated, therefore set to 0
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        Debug.Log(SceneManager.GetActiveScene().name);
        Debug.Log(canMove);
        if (sceneName == "Dungeon_1")
            playerStats.IncreaseHighScore(-playerStats.highScore);

        canMove = true;
    }

    void FixedUpdate()
    {
        //checks if the player can currently move, usually disabled by open menus.
        if (canMove)
        {
            //In the InputManager, when using WASD, arrow keys or the controller stick, the walkdirection will be saved as a vector
            Vector2 walkDirectionAsVector = InputManager.CalculateInputDirection();

            //Using advanced mathematic skills we were able to calculate the walking direction in degree from a vector.
            walkDirectionInDegree = Mathf.Atan2(walkDirectionAsVector.y, walkDirectionAsVector.x) * Mathf.Rad2Deg;
            //if the walking direction is > 0
            if (walkDirectionAsVector.magnitude > 0)
            {
                //first the current walkDirection will be saved as the lookDirection. This is important for the animator and the direction in which fireballs will be shot
                lookDirection = walkDirectionInDegree;
                //the melee hitbox and the spawn position of fireballs must move with the player accordingly
                attackPoint.transform.position = this.transform.position + new Vector3(walkDirectionAsVector.x, walkDirectionAsVector.y, 0);
                rangeAttackPoint.transform.position = this.transform.position + new Vector3(walkDirectionAsVector.x, walkDirectionAsVector.y, 0);
                lookDirectionAsVector = walkDirectionAsVector;
            }
            //"Hold shift to run"
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
            animator.SetFloat("a_Speed", rig.velocity.magnitude);
        }
    }

    private void Update()
    {
        if (canMove)
        {
            //M opens the map menu
            if (InputManager.GetActionDown(InputManager.map))
            {
                //if no mapeditor was assigned by now, it will try again
                if (mapeditor == null)
                {
                    AssignMapManager();
                }
                //if the inventory is currently not open, it opens the map
                if (!inventoryManager.activeInHierarchy)
                {
                    ShowHideMap();
                }
            }
            //I opens the inventory
            if (InputManager.GetActionDown(InputManager.inventory))
            {
                //if no inventorymanager is assigned it will try again
                if (inventoryManager == null)
                {
                    AssignInventoryManager();
                }
                //if the map menu is not open, it opens the inventory
                if (!mapeditor.activeInHierarchy)
                {
                    ShowHideInventory();
                }
            }
            //ESC closes currently open windows
            if(InputManager.GetActionDown(InputManager.cancel))
            {
                if (inventoryManager.activeInHierarchy)
                {
                    ShowHideInventory();
                }
                else if (mapeditor.activeInHierarchy)
                {
                    ShowHideMap();
                }
            }
            //if the game is currently not paused (through menus or dialogue)
            if (!GameTime.IsGamePaused)
            {
                //if E is pressed, it will check if something to interact is nearby
                if (InputManager.GetActionDown(InputManager.action))
                {
                    InteractWithInteractables();
                }
                //if space mouse is pressed, an attack will be executed
                if (InputManager.GetActionDown(InputManager.attack))
                {
                    playerCombatTEST.Attack();
                }
                //left mouse interacts and attacks at the same time
                if (InputManager.GetActionDown(InputManager.actionAndAttack))
                {
                    bool interactedSuccessfully = InteractWithInteractables();
                    if (!interactedSuccessfully)
                    {
                        playerCombatTEST.Attack();
                    }
                }

            }
            //ESC also opens the pause menu
            if (InputManager.GetActionDown(InputManager.cancel))
            {
                if (!Pause.activeSelf)
                {
                    Pause.SetActive(true);
                    GameTime.UpdateIsGamePaused();
                }
                else
                {
                    Pause.SetActive(false);
                    GameTime.UpdateIsGamePaused();
                }
            }
        }
    }

    /// <summary>
    /// Checks for interactables in range and interacts with them. Does nothing if nothing happens
    /// </summary>
    /// <returns>returns true, if successfully interacted, else false.</returns>
    private bool InteractWithInteractables()
    {
        //Find colliders in range
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();

        Physics2D.OverlapCircle(gameObject.transform.position, interactionRadius, contactFilter.NoFilter(), results);
        bool interactedSuccessfully = false;

        //Sort out all noninteractables
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
        //interact with the first interactable
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
            if (collider.tag.Equals("NPC"))
            {
                DialogueTrigger dT = collider.GetComponent<DialogueTrigger>();
                if(dT != null)
                {
                    dT.TriggerNPC();
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
        //this determines what map piece the player is on, so that it can't be dragged in the map menu 
        if (MapManager.Current != null)
        {
            MapPiece collidedWith = MapManager.Current.SearchMapPiece(collision.gameObject);
            if (collidedWith != null)
            {
                //the second dungeon has elemental elements that will be indicated by a border in the map menu and a camera tint
                Element element = MapManager.Current.FindElementOfMapPiece(collidedWith);

                CameraTintScript.Instance.SwitchColor(element);

                MapManager.Current.UpdatePlayerPosition(collidedWith);
            }
        }
    }

    /// <summary>
    /// Will be called if the player is hit by an enemy attack
    /// </summary>
    /// <param name="attackValue">the attack value of the attacker</param>
    /// <returns>Actual damage Taken</returns>
    public int TakeDamage(int attackValue, CharacterStats attacker)
    {
        return playerStats.TakeDamage(attackValue, attacker);
        
    }

    /// <summary>
    /// If the player's HP reach 0, the game over screen will be loaded
    /// </summary>
    public static void Die()
    {
        UIManager.instance.isGameOver = true;
    }
    /// <summary>
    /// Switches the state of the visual map between visible and invisible
    /// </summary>
    private void ShowHideMap()
    {
        if(mapeditor.activeInHierarchy)
        {
            MapManager.Current.PrepareForClosure();
            PlayerIcon.Instance.PauseAnimation();
        }
        else
        {
            PlayerIcon.Instance.UnpauseAnimation();
        }

        mapeditor.SetActive(!mapeditor.activeSelf);
        GameTime.UpdateIsGamePaused();
    }

    /// <summary>
    /// Sets the inventory to the opposite state (open vs closed) and resets the merge and remove buttons
    /// </summary>
    private void ShowHideInventory()
    {
        if(inventoryUI.activeInHierarchy)
        {
            Inventory.instance.ResetButtons();
        }
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        GameTime.UpdateIsGamePaused();
    }

    private void AssignMapManager()
    {
        mapeditor = MapManager.Current.gameObject;
    }

    private void AssignInventoryManager()
    {
        inventoryManager = InventoryManager.Current.gameObject;

        InventoryUI temp = FindObjectOfType<InventoryUI>();
        inventoryUI = temp.gameObject;

        Inventory.instance.Initialize();
        temp.Initialize();
    }
}
