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
        //Get all fighters here somehow. (Loading prefabs?)

        foreach(Fighter fighter in listFighters)
        {
            queueFighters.Enqueue(fighter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
