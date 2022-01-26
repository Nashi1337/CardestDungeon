using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static BattleMaster current;

    // Start is called before the first frame update
    void Start()
    {
        queueFighters = new Queue<Fighter>();
        listFighters = new List<Fighter>();

        //Create all fighters here somehow. (Loading prefabs?)

        GatherAllFighters();

        foreach(Fighter fighter in listFighters)
        {
            queueFighters.Enqueue(fighter);
        }

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
        Debug.Log("Bist wieder dran, " + queueFighters.Peek().gameObject);
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
        ActivateFighter();
    }

    /// <summary>
    /// Call this for attacking a fighter
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    public void AttackFighter(Fighter attacker, Fighter defender)
    {
        defender.GetAttacked(attacker.GetStatus().attack);
    }

    public Fighter[] GetAllFightersFromTeam(Fighter.Team team)
    {
        return listFighters.FindAll(x => x.GetTeam() == team).ToArray();
    }
}
