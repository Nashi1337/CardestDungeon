using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleMaster : MonoBehaviour
{

    public static BattleMaster Current
    {
        get
        {
            if(current == null)
            {
                current = FindObjectOfType<BattleMaster>();
            }
            return current;
        }
    }


    /// <summary>
    /// The tail is always the one who has its turn
    /// </summary>
    public Queue<Fighter> queueFighters;
    public List<Fighter> listFighters;
    
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private BattleHUD playerHUD;
    [SerializeField]
    private BattleHUD enemyHUD;
    [SerializeField]
    private Button[] playerActions;
    [SerializeField]
    private GameObject playerGround;
    [SerializeField]
    private GameObject enemyGround;

    private static BattleMaster current;

    // Start is called before the first frame update
    void Start()
    {
        queueFighters = new Queue<Fighter>();
        listFighters = new List<Fighter>();
        
        LoadFightersIntoScene();

        StartCoroutine(SetupBattle());

    }

    private void LoadFightersIntoScene()
    {
        GameObject player = BattleData.playerToLoad;
        player = Instantiate(player);
        player.name = player.name.Remove(player.name.LastIndexOf('(')); //Removes the "(clone)" that unity adds when instantiating prefabs
        player.transform.SetParent(playerGround.transform, true);

        GameObject enemy = BattleData.enemiesToLoad[0];
        enemy = Instantiate(enemy);
        enemy.name = enemy.name.Remove(enemy.name.LastIndexOf('(')); //Removes the "(clone)" that unity adds when instantiating prefabs
        enemy.transform.SetParent(enemyGround.transform, true);

        //Das ist blöd. Anderse machen
        Array.Find(playerActions, action => action.name == "attackbutton").onClick.AddListener(player.GetComponent<PlayerFighter>().OnAttackButton);
        Array.Find(playerActions, action => action.name == "healbutton").onClick.AddListener(player.GetComponent<PlayerFighter>().OnHealButton);
    }

    IEnumerator SetupBattle()
    {
        GatherAllFighters();

        foreach (Fighter fighter in listFighters)
        {
            queueFighters.Enqueue(fighter);
        }


        dialogueText.text = "A wild " + listFighters.Find(x => x.GetTeam() == Fighter.Team.Enemy).name + " appeared!";

        playerHUD.SetHUD(listFighters.Find(fighter => fighter.GetTeam() == Fighter.Team.Player));
        enemyHUD.SetHUD(listFighters.Find(fighter => fighter.GetTeam() == Fighter.Team.Enemy));

        //Wait for 2 seconds
        yield return new WaitForSeconds(0);

        //After the battle is set up it's the player's turn
        ActivateFighter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GatherAllFighters()
    {
        Fighter[] allFighters = FindObjectsOfType<Fighter>();
        foreach(Fighter fighter in allFighters)
        {
            listFighters.Add(fighter);
        }
    }

    /// <summary>
    /// Tells the current Fighter that it is his turn now.
    /// </summary>
    private void ActivateFighter()
    {
        queueFighters.Peek().ActivateMyTurn();
    }

    private void NextFighter()
    {
        
        queueFighters.Enqueue(queueFighters.Dequeue());
    }

    /// <summary>
    /// Call this after you finished your turn.
    /// </summary>
    public void EndTurn()
    {
        NextFighter();

        if (listFighters.Count == 1)
        {
            if (listFighters[0].GetTeam() == Fighter.Team.Enemy)
            {
                WriteToDialogue("You lost!");
            }
            else
            {
                WriteToDialogue("You won!");
            }
        }
        else
        {
            ActivateFighter();
        }
    }

    /// <summary>
    /// Call this for attacking a fighter. Attacks fighter and updates health bar of the defender
    /// </summary>
    /// <param name="attackstrength">The strength whith which will be attacked.</param>
    /// <param name="defender">The one who gets attacked</param>
    public void AttackFighter(Fighter attacker, Fighter defender, int attackstrength)
    {
        int actualDamage = defender.GetAttacked(attackstrength);

        WriteToDialogue(attacker.name + " attacked " + defender.name + " with " + actualDamage + " damage!");
    }

    /// <summary>
    /// Heals the given fighter by the given amount
    /// </summary>
    /// <param name="fighter">the fighter to be healed</param>
    /// <param name="healed">How much health should be healed</param>
    public void HealFighter(Fighter fighter, int healed)
    {
        int healedHealth = fighter.GetHealed(healed);

        if(healedHealth != healed)
        {
            WriteToDialogue(fighter.name + " was fully healed!");
        }
        else
        {
            WriteToDialogue(fighter.name + " healed " + healed + " health points!");
        }
    }

    public Fighter[] GetAllFightersFromTeam(Fighter.Team team)
    {
        return listFighters.FindAll(x => x.GetTeam() == team).ToArray();
    }

    /// <summary>
    /// Destroys a fighter fully. Always call this when a fighter should be removed from scene (e.g. when dead).
    /// </summary>
    /// <param name="fighter">the fighter script of the GameObject to destroy</param>
    public void DestroyFighter(Fighter fighter)
    {
        Fighter subject = queueFighters.Dequeue();
        while (subject != fighter)
        {
            queueFighters.Enqueue(subject);
            subject = queueFighters.Dequeue();
        }

        listFighters.Remove(subject);

        Destroy(fighter.gameObject);
    }

    public void ActivatePlayerActions()
    {
        foreach (Button button in playerActions)
        {
            button.interactable = true;
        }
    }

    public void DeactivatePlayerActions()
    {
        foreach (Button button in playerActions)
        {
            button.interactable = false;
        }
    }

    public void WriteToDialogue(string message)
    {
        dialogueText.text = message;
    }
}
