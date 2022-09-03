using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{

    void Start()
    {
        UpdateStats();
        Initialize();
    }

    public override void UpdateStats()
    {
        base.UpdateStats();
    }

    public override int TakeDamage(int attackValue, CharacterStats attacker)
    {
        //Knockback Calculation
        float knockbackForce = knockbackModifier * attackValue;
        Vector2 knockbackDirection = (transform.position - attacker.transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        return base.TakeDamage(attackValue, Defense, attacker);
    }
}
