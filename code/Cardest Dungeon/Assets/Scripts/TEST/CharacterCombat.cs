using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    private bool attackAvailable = true;
    [SerializeField]
    private float attackRate = 2f;

    public event System.Action OnAttack;

    CharacterStats myStats;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(attackAvailable && collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(myStats.Attack);
            attackAvailable = false;
            StartCoroutine(AttackCooldown());
        }
    }

    //public void Attack(CharacterStats targetStats)
    //{
    //    if(attackRate <= 0f)
    //    {
    //        StartCoroutine(DoDamage(targetStats, attackDelay));

    //        //? macht eine != null Abfrage vor dem aufruf
    //        OnAttack?.Invoke();

    //        attackRate = 1f / attackSpeed;
    //    }
    //}

    //IEnumerator DoDamage(CharacterStats stats, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    stats.TakeDamage(myStats.Attack);
    //}

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        attackAvailable = true;
    }
}
