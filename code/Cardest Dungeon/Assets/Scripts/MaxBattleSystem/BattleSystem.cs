using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerbattleui;
    public GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [SerializeField]
    private Transform playerBattleStation;
    [SerializeField]
    private Transform enemyBattleStation;
    [SerializeField]
    private Button[] playerActions;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //DEBUG
        GameObject playergameobject = Instantiate(playerbattleui);
        playergameobject.transform.parent = playerBattleStation;
        playergameobject.transform.localPosition = new Vector3(0, 1.5f, 0);
        playerUnit = playergameobject.GetComponent<Unit>();

        GameObject enemygameobject = Instantiate(enemyPrefab);
        enemygameobject.transform.parent = enemyBattleStation;
        enemygameobject.transform.localPosition = new Vector3(0, 1.5f, 0);
        enemyUnit = enemygameobject.GetComponent<Unit>();
        //DEBUG ENDE
        

        dialogueText.text = "A wild " + enemyUnit.unitName + " appeared!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        //Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        //After the battle is set up it's the player's turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {

        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHealth(enemyUnit.currentHP);
        dialogueText.text = "Player attacked " + enemyUnit.unitName + " for " + playerUnit.damage;

        foreach(Button button in playerActions)
        {
            button.interactable = false;
        }

        //Sobald angegriffen wurde wird die Interaktivitaet des AttackButtons auf false gesetzt
        //Das soll davor schuetzen, dass man wenn man nicht dran ist nicht am Button rumspielt


        yield return new WaitForSeconds(2f);
        // Check if enemy is dead
        if (isDead)
        {
            // End the battle
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //Enemy turn


            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        
        //Change state based on what happened
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Player won the battle!";
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "Player was defeated!";
        }

    }

    IEnumerator EnemyTurn()
    {
        //TODO: Implementiere kuenstliche Intelligenz, die basierend auf den Informationen die sie ueber den Enemy und den Player hat determiniert welche Aktion ausgefuehrt werden soll.
        //Moeglichkeiten:
        //Angriff, Heilen, Fliehen
        //Fortgeschrittene Moeglichkeiten (Boss, zweiter Dungeon etc.):
        //Spells, Items (Verteidigen?), Verstaerkung rufen, Suizid begehen


        dialogueText.text = enemyUnit.unitName + " attacks for " + enemyUnit.damage + "!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHealth(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        //am Anfang des PlayerTurns wird die Interaktivitaet des AttackButtons auf true gesetzt
        foreach (Button button in playerActions)
        {
            button.interactable = true;
        }
        dialogueText.text = "Choose an action: ";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.magic);

        playerHUD.SetHealth(playerUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " healed themself for " + playerUnit.magic + " Health Points!";

        foreach(Button button in playerActions)
        {
            button.interactable = false;
        }
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
}
