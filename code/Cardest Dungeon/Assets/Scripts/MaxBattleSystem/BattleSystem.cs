using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURM, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerbattleui;
    public GameObject enemybattleui;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [SerializeField]
    public Button AttackButton;
    [SerializeField]
    public Button HealButton;
    [SerializeField]
    private string dungeonscenename;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playergameobject = Instantiate(playerbattleui, playerBattleStation);
        //playergameobject.transform.localScale = Vector3.one;
        //das soll den player und enemy sprite als child der jeweiligen battle stations spawnen lassen
        playerUnit = playergameobject.GetComponent<Unit>();

        GameObject enemygameobject = Instantiate(enemybattleui, enemyBattleStation);
        enemygameobject.transform.localScale = Vector3.one;
        enemyUnit = enemygameobject.GetComponent<Unit>();

        

        dialogueText.text = "A wild " + enemyUnit.unitName + " appeared!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        //warte 2 Sekunden

        state = BattleState.PLAYERTURN;
        //nach dem der Battle upgesettet wurde wird der naechste Status PLAYERTURN
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {

        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Player attacked " + enemyUnit.unitName + " for " + playerUnit.damage;

        AttackButton.interactable = false;
        HealButton.interactable = false;
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


            state = BattleState.ENEMYTURM;
            StartCoroutine(EnemyTurn());
        }
        
        //Change state based on what happened
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Player won the battle!";
            SceneManager.LoadScene(dungeonscenename);
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

        playerHUD.SetHP(playerUnit.currentHP);

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
        AttackButton.interactable = true;
        HealButton.interactable = true;
        dialogueText.text = "Choose an action: ";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(playerUnit.magic);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " healed themself for " + playerUnit.magic + " Health Points!";

        AttackButton.interactable = false;
        HealButton.interactable = false;

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURM;
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
