using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private class EnemyData
    {
        public int index;
        public bool isDead;

    
        public EnemyData(int index)
        {
            this.index = index;
        }    
    }

    public static EnemyManager enemymanager;

    private static List<EnemyData> enemies;




    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (enemymanager == null)
        {
            enemymanager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        enemies = new List<EnemyData>();
        DungeonEnemy[] ballenemies = FindObjectsOfType<DungeonEnemy>();

        Debug.Log("Hier sind alle Gegner die ich gefunden habe: " + ballenemies);

        foreach(DungeonEnemy enemy in ballenemies)
        {
            Debug.Log("Hallo, ich bin " + enemy + " und ich habe die Nummer " + enemy.GetIndex());
            EnemyData data = enemies.Find(e => e.index == enemy.GetIndex());
            if (data!=null && data.isDead)
            {
                DeleteEnemy(enemy.GetIndex());
            }
        }
    }

    private void Start()
    {
        DungeonEnemy[] allenemies = FindObjectsOfType<DungeonEnemy>();
        foreach(DungeonEnemy enemy in allenemies)
        {
            enemies.Add(new EnemyData(enemy.GetIndex()));
        }
    }

    private EnemyData FindEnemyDataByIndex(int index)
    {
        return enemies.Find(e => e.index == index);
    }

    public void DeleteEnemy(int index)
    {
        EnemyData delete = FindEnemyDataByIndex(index);
        delete.isDead = true;
    }
}
