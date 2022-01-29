using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Leads the battle. The BattleMaster is the referee of the fight and the intermediary for all fighters.
/// It handles the turn-order, tells the fighters when it is their turn and awaits a call back when the fighter is finished.
/// It also handles attacking and should handle any other mean that involves more than the active fighter itself.
/// It checks for winning/loosing condition and also sets up the battle stage before fight.
/// </summary>
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
    private string dungeonSceneName;
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

    /// <summary>
    /// Loads and sets up the fighters into the scene which are saved in the class BattlData
    /// </summary>
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

    /// <summary>
    /// Sets up it's data structures and the HUD fpr the battle. Afterwards it activates the first fighter in the queue.
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// Gathers all instances of the class "Fighter" that are in the scene and saves them in listFighters.
    /// </summary>
    private void GatherAllFighters()
    {
        Fighter[] allFighters = FindObjectsOfType<Fighter>();
        foreach(Fighter fighter in allFighters)
        {
            listFighters.Add(fighter);
        }
    }

    /// <summary>
    /// Tells the current Fighter that it is his turn now. This method requires the EndTurn() method to be called by the now active fighter
    /// after its turn.
    /// </summary>
    private void ActivateFighter()
    {
        queueFighters.Peek().ActivateMyTurn();
    }

    /// <summary>
    /// shifts the queue by one. As such the old tail is now the new head.
    /// </summary>
    private void NextFighter()
    {
        queueFighters.Enqueue(queueFighters.Dequeue());
    }

    /// <summary>
    /// Ends the turn of the currently active fighter, checks for winning and loosing condition and reacts accordingly 
    /// and calls the next active fighter
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
                StartCoroutine(ReturnToDungeon());
            }
        }
        else
        {
            ActivateFighter();
        }
    }

    IEnumerator ReturnToDungeon()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(dungeonSceneName);
    }

    /// <summary>
    /// Conveys an attack by attacker against defender with strength attackstrength and sends a corresponding message to the dialogue window.
    /// </summary>
    /// <param name="attackstrength">The strength whith which will be attacked</param>
    /// <param name="defender">The fighter who is being attacked</param>
    /// <param name="attacker">The fighter who is attacking</param>
    public void AttackFighter(Fighter attacker, Fighter defender, int attackstrength)
    {
        int actualDamage = defender.GetAttacked(attackstrength);

        WriteToDialogue(attacker.name + " attacked " + defender.name + " with " + actualDamage + " damage!");
    }

    /// <summary>
    /// Heals fighter by hp points and sends a corresponding message to the dialogue window.
    /// </summary>
    /// <param name="fighter">The fighter that will be healed</param>
    /// <param name="hp">the amount of health points that should be healed</param>
    public void HealFighter(Fighter fighter, int hp)
    {
        int healedHealth = fighter.GetHealed(hp);

        if(healedHealth != hp)
        {
            WriteToDialogue(fighter.name + " was fully healed!");
        }
        else
        {
            WriteToDialogue(fighter.name + " healed " + hp + " health points!");
        }
    }

    /// <summary>
    /// Searches for all fighters within the team team and returns them.
    /// </summary>
    /// <param name="team">The team in which a fighter must be in order to be returned</param>
    /// <returns>An array with all fighters that are part of the given team.</returns>
    public Fighter[] FindAllFightersFromTeam(Fighter.Team team)
    {
        return listFighters.FindAll(x => x.GetTeam() == team).ToArray();
    }

    /// <summary>
    /// Removes fighter from the queue and from the list and destroys the GameObject of this fighter instance
    /// </summary>
    /// <param name="fighter">The fighter which should be destroyed</param>
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

    /// <summary>
    /// Makes all buttons that represent player actions interactable
    /// </summary>
    public void ActivatePlayerActions()
    {
        foreach (Button button in playerActions)
        {
            button.interactable = true;
        }
    }

    /// <summary>
    /// Makes all buttons that represent player actions uninteractable
    /// </summary>
    public void DeactivatePlayerActions()
    {
        foreach (Button button in playerActions)
        {
            button.interactable = false;
        }
    }


    /// <summary>
    /// Writes a message to the dialogue window that is shown during a fight.
    /// </summary>
    /// <param name="message"></param>
    public void WriteToDialogue(string message)
    {
        dialogueText.text = message;
    }
}
