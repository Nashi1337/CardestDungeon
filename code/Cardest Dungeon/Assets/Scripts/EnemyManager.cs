using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages all enemies in the dungeon and their spawning.
/// </summary>
public class EnemyManager : MonoBehaviour
{

    /// <summary>
    /// Contains important information about an enemy.
    /// </summary>
    private class EnemyData
    {
        public int index;
        public bool isDead;

    
        public EnemyData(int index)
        {
            this.index = index;
            isDead = false;
        }    
    }

    public static EnemyManager enemymanager;

    private List<EnemyData> enemies;




    private void Awake()
    {
        if (enemymanager == null)
        {
            enemymanager = this;
            DontDestroyOnLoad(gameObject);

            enemies = new List<EnemyData>();
            DungeonEnemy[] allenemies = FindObjectsOfType<DungeonEnemy>();

            foreach (DungeonEnemy enemy in allenemies)
            {
                enemies.Add(new EnemyData(enemy.GetIndex()));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">The identifier for the enemy that will be looked for.</param>
    /// <returns>Returns a reference of enemyData that corresponds to the given index.</returns>
    private EnemyData GetEnemyData(int index)
    {
        return enemies.Find(e => e.index == index);
    }

    public void KillEnemy(int index)
    {
        EnemyData victim = GetEnemyData(index);
        victim.isDead = true;
    }

    /// <summary>
    /// Checks if the enemy with the given index should be dead.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>True if dead, else falls.</returns>
    public bool HasMyTimeCome(int index)
    {
        return GetEnemyData(index).isDead;
    }
}
