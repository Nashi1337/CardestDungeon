using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an appearance effect and starts the enemy regularly afterwards.
/// </summary>
public class EnemyAppears : MonoBehaviour
{
    private static float timeInSeconds = 1;

    private float alpha;
    private Collider2D[] colliders;
    private MonoBehaviour[] scripts;
    private Enemy enemyScript;
    private SpriteRenderer ren;
    private void Start()
    {
        colliders = GetComponents<Collider2D>();
        enemyScript = GetComponent<Enemy>();
        scripts = GetComponents<MonoBehaviour>();
        ren = GetComponent<SpriteRenderer>();

        foreach(Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        enemyScript.Initialize();
        enemyScript.SetScale();
        foreach(MonoBehaviour mono in scripts)
        {
            if (mono != this)
            {
                mono.enabled = false;
            }
        }

        Color temp = ren.color;
        temp.a = 0;
        ren.color = temp;
    }

    private void LateUpdate()
    {
        alpha += Time.deltaTime/timeInSeconds;

        Color temp = ren.color;
        temp.a = alpha;
        ren.color = temp;

        if(alpha >= 1)
        {
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }

            foreach (MonoBehaviour mono in scripts)
            {
                mono.enabled = true;
            }

            Destroy(this);
        }
    }
}
