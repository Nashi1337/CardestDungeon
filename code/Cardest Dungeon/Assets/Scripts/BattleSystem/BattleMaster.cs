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
    /// <param name="attackstrength">The strength whith which will be attacked.</param>
    /// <param name="defender">The one who gets attacked</param>
    public void AttackFighter(Fighter defender, int attackstrength)
    {
        defender.GetAttacked(attackstrength);
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
}
